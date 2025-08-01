using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TesteDengine;
using TesteDengine.Application.Relatorios.Interfaces;
using TesteDengine.Application.Services;
using TesteDengine.Application.Services.DTOs;
using TesteDengine.Application.Services.Interfaces;
using TesteDengine.Domain.Entities;
using TesteDengine.Domain.Interfaces;

public class Program
{
    private static bool BancoPopulado = false;

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Bem-vindo ao sistema de Faturas!");

        var consoleServiceProvider = new ConsoleServiceProvider();
        var serviceProvider = consoleServiceProvider.GetServiceProvider();

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DbExercicio4>();
            await dbContext.Database.EnsureCreatedAsync();
            Console.WriteLine("Banco de dados em memória OK.");
        }

        await RunMenu(serviceProvider);

        Console.WriteLine("\nPressione qualquer tecla para sair...");
        Console.ReadKey();
    }

    private static async Task RunMenu(IServiceProvider serviceProvider)
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("-=-= Menu Principal -=-=");
            Console.WriteLine("1. Gerenciar Clientes");
            Console.WriteLine("2. Gerenciar Faturas");
            Console.WriteLine("3. Gerar Relatórios");
            if (!BancoPopulado)
            {
                Console.WriteLine("4. Inserir Dados de Teste (APENAS PARA FINS DE TESTES)");
            }
            Console.WriteLine("0. Sair");
            Console.Write("Escolha uma opção: ");

            var choice = Console.ReadLine();

            using (var scope = serviceProvider.CreateScope())
            {
                var clienteService = scope.ServiceProvider.GetRequiredService<IClienteService>();
                var faturaService = scope.ServiceProvider.GetRequiredService<IFaturaService>();
                var faturaItemService = scope.ServiceProvider.GetRequiredService<IFaturaItemService>();
                var faturaRelatorio = scope.ServiceProvider.GetRequiredService<IFaturaRelatorioService>();

                var faturaRelatorioService = serviceProvider.GetRequiredService<IFaturaRelatorioService>();

                

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await ManageClientes(clienteService);
                            break;
                        case "2":
                            await ManageFaturas(faturaService, faturaItemService, clienteService);
                            break;
                        case "3":
                            await GenerateReports(faturaRelatorio);
                            break;
                        case "4":
                            if (BancoPopulado)
                            {
                                Console.WriteLine("Dados no banco já foram inseridos. Pressione qualquer tecla para tentar novamente.");
                                Console.ReadKey();
                                break;
                            }
                            var clienteRepo = serviceProvider.GetRequiredService<IClienteRepository>();
                            var faturaRepo = serviceProvider.GetRequiredService<IFaturaRepository>();
                            var itemRepo = serviceProvider.GetRequiredService<IFaturaItemRepository>();
                            await InserirDadosDeTeste(clienteRepo, faturaRepo, itemRepo);
                            break;
                        case "0":
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Pressione qualquer tecla para tentar novamente.");
                            Console.ReadKey();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nOcorreu um erro: {ex.Message}");
                    Console.WriteLine("Pressione qualquer tecla para continuar.");
                    Console.ReadKey();
                }
            }
        }
    }

    private static async Task ManageClientes(IClienteService clienteService)
    {
        bool managing = true;
        while (managing)
        {
            Console.Clear();
            Console.WriteLine("-=-= Gerenciar Clientes -=-=");
            Console.WriteLine("1. Adicionar Novo Cliente");
            Console.WriteLine("2. Listar Todos os Clientes");
            Console.WriteLine("3. Buscar Cliente por ID");
            Console.WriteLine("4. Atualizar Cliente");
            Console.WriteLine("5. Excluir Cliente");
            Console.WriteLine("0. Voltar ao Menu Principal");
            Console.Write("Escolha uma opção: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Digite o nome do cliente: ");
                        var nomeCliente = Console.ReadLine();
                        await clienteService.AddAsync(new ClienteCreateDTO { Nome = nomeCliente });
                        Console.WriteLine($"Cliente '{nomeCliente}' adicionado com sucesso!");
                        break;
                    case "2":
                        Console.WriteLine("\n-=-= Lista de Clientes -=-=");
                        var clientes = await clienteService.GetAllAsync();
                        if (!clientes.Any())
                        {
                            Console.WriteLine("Nenhum cliente cadastrado.");
                        }
                        else
                        {
                            foreach (var c in clientes)
                            {
                                Console.WriteLine($"ID: {c.ClienteId}, Nome: {c.Nome}");
                            }
                        }
                        break;
                    case "3":
                        Console.Write("Digite o ID do cliente: ");
                        if (int.TryParse(Console.ReadLine(), out int clienteId))
                        {
                            var cliente = await clienteService.GetByIdAsync(clienteId);
                            if (cliente != null)
                            {
                                Console.WriteLine($"ID: {cliente.ClienteId}, Nome: {cliente.Nome}");
                            }
                            else
                            {
                                Console.WriteLine("Cliente não encontrado.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID inválido.");
                        }
                        break;
                    case "4":
                        Console.Write("Digite o ID do cliente para atualizar: ");
                        if (int.TryParse(Console.ReadLine(), out int updateClienteId))
                        {
                            Console.Write($"Digite o novo nome: ");
                            var novoNome = Console.ReadLine();

                            await clienteService.UpdateAsync(new ClienteUpdateDTO { ClienteId = updateClienteId, Nome = novoNome });
                            Console.WriteLine("Cliente atualizado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("ID inválido.");
                        }
                        break;
                    case "5":
                        Console.Write("Digite o ID do cliente para excluir: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteClienteId))
                        {
                            Console.Write($"Tem certeza que deseja excluir o cliente ID {deleteClienteId}? (s/n): ");
                            if (Console.ReadLine()?.ToLower() == "s")
                            {
                                await clienteService.DeleteAsync(deleteClienteId);
                                Console.WriteLine("Cliente excluído com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("Exclusão cancelada.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID inválido.");
                        }
                        break;
                    case "0":
                        managing = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao gerenciar cliente: {ex.Message}");
            }
            if (managing)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private static async Task ManageFaturas(IFaturaService faturaService, IFaturaItemService faturaItemService, IClienteService clienteService)
    {
        bool managing = true;
        while (managing)
        {
            Console.Clear();
            Console.WriteLine("-=-= Gerenciar Faturas -=-=");
            Console.WriteLine("1. Adicionar Nova Fatura");
            Console.WriteLine("2. Adicionar Item à Fatura");
            Console.WriteLine("3. Atualizar Item da Fatura");
            Console.WriteLine("4. Remover Item da Fatura");
            Console.WriteLine("5. Listar Todas as Faturas");
            Console.WriteLine("6. Listar Faturas por Cliente");
            Console.WriteLine("7. Buscar Fatura por ID (com detalhes)");
            Console.WriteLine("8. Atualizar Fatura");
            Console.WriteLine("9. Excluir Fatura");
            Console.WriteLine("0. Voltar ao Menu Principal");
            Console.Write("Escolha uma opção: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Digite o ID do Cliente para a fatura: ");
                        if (int.TryParse(Console.ReadLine(), out int clienteId))
                        {
                            await faturaService.AddAsync(new FaturaCreateDTO { ClienteId = clienteId, Data = DateTime.Now });
                            Console.WriteLine($"Fatura criada para o cliente ID {clienteId}!");
                        }
                        else
                        {
                            Console.WriteLine("ID de Cliente inválido.");
                        }
                        break;
                    case "2": 
                        Console.Write("Digite o ID da Fatura: ");
                        if (!int.TryParse(Console.ReadLine(), out int faturaIdAddItem))
                        {
                            Console.WriteLine("ID de Fatura inválido. Por favor, digite um número.");
                            break;
                        }

                        Console.Write("Ordem do item (múltiplo de 10, sem buracos): ");
                        if (!int.TryParse(Console.ReadLine(), out int ordemItem))
                        {
                            Console.WriteLine("Ordem inválida. Por favor, digite um número inteiro.");
                            break;
                        }

                        Console.Write("Valor do item: ");
                        if (!double.TryParse(Console.ReadLine(), out double valorItem))
                        {
                            Console.WriteLine("Valor inválido. Por favor, digite um número.");
                            break;
                        }

                        if (valorItem > 1000)
                        {
                            Console.WriteLine("Valor acima de 1000. Confirmar valor? (s/n):");
                            if (Console.ReadLine()?.ToLower() != "s")
                            {
                                Console.WriteLine("Adição de item cancelada pelo usuário.");
                                break; 
                            }
                        }

                        Console.Write("Descrição do item (máx 20 caracteres): ");
                        var descricaoItem = Console.ReadLine();

                        try
                        {
                            await faturaItemService.AddAsync(new FaturaItemCreateDTO {
                                FaturaId = faturaIdAddItem,
                                Ordem = ordemItem,
                                Valor = valorItem,
                                Descricao = descricaoItem
                            });
                            Console.WriteLine("Item adicionado com sucesso!");
                        }
                        catch (KeyNotFoundException ex)
                        {
                            Console.WriteLine($"Erro: {ex.Message}");
                        }
                        catch (ValidationException ex) 
                        {
                            Console.WriteLine($"Erro de validação: {ex.Message}");
                        }
                        catch (Exception ex) 
                        {
                            Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
                        }
                        break;
                    case "3": // Atualizar Item da Fatura
                        Console.Write("Digite o ID da Fatura à qual o item pertence: ");
                        if (!int.TryParse(Console.ReadLine(), out int faturaIdUpdateItem))
                        {
                            Console.WriteLine("ID de Fatura inválido. Por favor, digite um número.");
                            break;
                        }

                        Console.Write("Digite o ID do Item da Fatura para atualizar: ");
                        if (!int.TryParse(Console.ReadLine(), out int faturaItemIdToUpdate))
                        {
                            Console.WriteLine("ID do Item inválido. Por favor, digite um número.");
                            break;
                        }

                        Console.Write("Nova Ordem do item (múltiplo de 10, sem buracos): ");
                        if (!int.TryParse(Console.ReadLine(), out int novaOrdemItem))
                        {
                            Console.WriteLine("Ordem inválida. Por favor, digite um número inteiro.");
                            break;
                        }

                        Console.Write("Novo Valor do item: ");
                        if (!double.TryParse(Console.ReadLine(), out double novoValorItem))
                        {
                            Console.WriteLine("Valor inválido. Por favor, digite um número.");
                            break;
                        }

                        if (novoValorItem > 1000)
                        {
                            Console.WriteLine("Novo valor acima de 1000. Confirmar valor? (s/n):");
                            if (Console.ReadLine()?.ToLower() != "s")
                            {
                                Console.WriteLine("Atualização de item cancelada pelo usuário.");
                                break;
                            }
                        }

                        Console.Write("Nova Descrição do item (máx 20 caracteres): ");
                        var novaDescricaoItem = Console.ReadLine();

                        try
                        {
                            await faturaItemService.UpdateAsync(new FaturaItemUpdateDTO {
                                FaturaItemId = faturaItemIdToUpdate,
                                Ordem = novaOrdemItem,
                                Valor = novoValorItem,
                                Descricao = novaDescricaoItem
                            });
                            Console.WriteLine("Item atualizado com sucesso!");
                        }
                        catch (KeyNotFoundException ex)
                        {
                            Console.WriteLine($"Erro: {ex.Message}");
                        }
                        catch (ValidationException ex)
                        {
                            Console.WriteLine($"Erro de validação: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Ocorreu um erro inesperado: {ex.Message}");
                        }
                        break;
                    case "4": // Remover Item
                        Console.Write("Digite o ID da Fatura: ");
                        if (!int.TryParse(Console.ReadLine(), out int faturaIdRemoveItem))
                        {
                            Console.WriteLine("ID de Fatura inválido. Por favor, digite um número.");
                            break;
                        }
                        Console.Write("Digite o ID do Item da Fatura para remover: ");
                        if (int.TryParse(Console.ReadLine(), out int faturaItemIdToRemove))
                            {
                                Console.Write($"Tem certeza que deseja remover o item ID {faturaItemIdToRemove} da fatura ID {faturaIdRemoveItem}? (s/n): ");
                                if (Console.ReadLine()?.ToLower() == "s")
                                {
                                    await faturaItemService.DeleteAsync(faturaIdRemoveItem);
                                    Console.WriteLine("Item removido com sucesso!");
                                }
                                else
                                {
                                    Console.WriteLine("Remoção de item cancelada.");
                                }
                            }
                        else { Console.WriteLine("ID do Item inválido."); }
                        
                        break;
                    case "5": // Listar todas
                        Console.WriteLine("\n-=-= Todas as Faturas -=-=");
                        var allFaturas = await faturaService.GetAllWithDetailsAsync();
                        if (!allFaturas.Any())
                        {
                            Console.WriteLine("Nenhuma fatura cadastrada.");
                        }
                        else
                        {
                            foreach (var fatura in allFaturas)
                            {
                                Console.WriteLine($"ID: {fatura.FaturaId}, Data: {fatura.Data.ToShortDateString()}, Cliente: {fatura.Cliente?.Nome}, Total: {fatura.Total:C}");
                            }
                        }
                        break;
                    case "6": // Listar por cliente
                        Console.Write("Digite o ID do Cliente para listar as faturas: ");
                        if (int.TryParse(Console.ReadLine(), out int clienteIdList))
                        {
                            var faturasDoCliente = await faturaService.GetAllByClienteIdAsync(clienteIdList);
                            if (!faturasDoCliente.Any())
                            {
                                Console.WriteLine($"Nenhuma fatura encontrada para o cliente ID {clienteIdList}.");
                            }
                            else
                            {
                                Console.WriteLine($"\n-=-= Faturas do Cliente ID {clienteIdList} -=-=");
                                foreach (var fatura in faturasDoCliente)
                                {
                                    Console.WriteLine($"ID: {fatura.FaturaId}, Data: {fatura.Data.ToShortDateString()}, Total: {fatura.Total:C}");
                                    foreach (var item in fatura.Itens)
                                    {
                                        Console.WriteLine($"  -> ID: {item.FaturaId}, Ordem: {item.Ordem}, Desc: {item.Descricao}, Valor: {item.Valor:C}, Verificação: {item.PrecisaVerificacao}");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID de Cliente inválido.");
                        }
                        break;
                    case "7": // Buscar por ID
                        Console.Write("Digite o ID da Fatura para detalhes: ");
                        if (int.TryParse(Console.ReadLine(), out int faturaIdDetails))
                        {
                            var faturaDetalhada = await faturaService.GetByIdAsync(faturaIdDetails);
                            if (faturaDetalhada != null)
                            {
                                Console.WriteLine($"\n-=-= Detalhes da Fatura ID {faturaDetalhada.FaturaId} -=-=");
                                Console.WriteLine($"  Cliente: {faturaDetalhada.Cliente?.Nome}");
                                Console.WriteLine($"  Data: {faturaDetalhada.Data.ToShortDateString()}");
                                Console.WriteLine($"  Total Geral: {faturaDetalhada.Total:C}");
                                Console.WriteLine("  Itens:");
                                if (!faturaDetalhada.Itens.Any())
                                {
                                    Console.WriteLine("    Nenhum item nesta fatura.");
                                }
                                else
                                {
                                    foreach (var item in faturaDetalhada.Itens.OrderBy(i => i.Ordem))
                                    {
                                        Console.WriteLine($"    - Ordem: {item.Ordem}, Desc: {item.Descricao}, Valor: {item.Valor:C}, Verificação: {item.PrecisaVerificacao}");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Fatura não encontrada.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID de Fatura inválido.");
                        }
                        break;
                    case "8": // Atualizar fatura
                        Console.Write("Digite o ID da fatura para atualizar: ");
                        if (int.TryParse(Console.ReadLine(), out int updateFaturaId))
                        {
                            var faturaToUpdate = await faturaService.GetByIdWithDetailsAsync(updateFaturaId);
                            if (faturaToUpdate == null)
                            {
                                Console.WriteLine("Fatura não encontrada.");
                                break;
                            }

                            Console.Write($"Cliente atual: {faturaToUpdate.Cliente?.Nome}. Digite o NOVO ID do Cliente para a fatura (ou ENTER para manter o atual): ");
                            var newClienteIdInput = Console.ReadLine();
                            int newClienteId = faturaToUpdate.Cliente.ClienteId; 
                            if (!string.IsNullOrWhiteSpace(newClienteIdInput))
                            {
                                if (int.TryParse(newClienteIdInput, out int parsedClienteId))
                                {
                                    var clienteExists = await clienteService.GetByIdAsync(parsedClienteId);
                                    if (clienteExists == null)
                                    {
                                        Console.WriteLine("Novo Cliente ID não encontrado. Operação cancelada.");
                                        break;
                                    }
                                    newClienteId = parsedClienteId;
                                }
                                else
                                {
                                    Console.WriteLine("ID de Cliente inválido. Operação cancelada.");
                                    break;
                                }
                            }

                            Console.Write($"Data atual: {faturaToUpdate.Data:dd/MM/yyyy}. Digite a NOVA Data (DD/MM/AAAA, ou ENTER para manter a atual): ");
                            var newDateInput = Console.ReadLine();
                            DateTime newDate = faturaToUpdate.Data; 
                            if (!string.IsNullOrWhiteSpace(newDateInput))
                            {
                                if (DateTime.TryParseExact(newDateInput, "dd/MM/yyyy",
                                    CultureInfo.GetCultureInfo("pt-BR"), DateTimeStyles.None, out DateTime parsedDate))
                                {
                                    newDate = parsedDate;
                                }
                                else
                                {
                                    Console.WriteLine("Formato de data inválido. Use o formato DD/MM/AAAA.");
                                    break;
                                }
                            }

                            await faturaService.UpdateAsync(new FaturaUpdateDTO {
                                FaturaId = updateFaturaId,
                                ClienteId = newClienteId,
                                Data = newDate
                            });
                            Console.WriteLine("Fatura atualizada com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("ID inválido.");
                        }
                        break;
                    case "9": // Excluir fatura
                        Console.Write("Digite o ID da fatura para excluir: ");
                        if (int.TryParse(Console.ReadLine(), out int deleteFaturaId))
                        {
                            Console.Write($"Tem certeza que deseja excluir a fatura ID {deleteFaturaId}? (s/n): ");
                            if (Console.ReadLine()?.ToLower() == "s")
                            {
                                await faturaService.DeleteAsync(deleteFaturaId);
                                Console.WriteLine("Fatura excluída com sucesso!");
                            }
                            else
                            {
                                Console.WriteLine("Exclusão cancelada.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("ID inválido.");
                        }
                        break;
                    case "0":
                        managing = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao gerenciar fatura: {ex.Message}");
            }
            if (managing)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private static async Task GenerateReports(IFaturaRelatorioService faturaRelatorioService)
    {
        bool reporting = true;
        while (reporting)
        {
            Console.Clear();
            Console.WriteLine("-=-= Gerar Relatórios -=-=");
            Console.WriteLine("1. Total por Cliente");
            Console.WriteLine("2. Total por Ano/Mês");
            Console.WriteLine("3. Top 10 Maiores Faturas");
            Console.WriteLine("4. Top 10 Maiores Itens");
            Console.WriteLine("0. Voltar ao Menu Principal");
            Console.Write("Escolha uma opção: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("\n-=-= Relatório: Total por Cliente -=-=");
                        try
                        {
                            var totalCliente = await faturaRelatorioService.ObterTotalPorClienteAsync();

                            foreach (var total in totalCliente)
                            {
                                Console.WriteLine($"- Cliente: {total.ClienteNome} (ID: {total.ClienteId}) - Total: {total.Total:C}");
                            }
                        }
                        catch (KeyNotFoundException ex)
                        {
                            Console.WriteLine($"Erro, nenhum cliente cadastrado!");
                        }

                        
                        break;
                    case "2":
                        Console.WriteLine("\n-=-= Relatório: Total por Ano/Mês -=-=");
                        try
                        {
                            var allFaturas = await faturaRelatorioService.ObterTotalPorMesAnoAsync();
                            foreach (var item in allFaturas)
                            {
                                Console.WriteLine($"- {item.Ano}/{item.Mes:00} - Total: {item.Total:C}");
                            }
                        }
                        catch (KeyNotFoundException ex)
                        {
                            Console.WriteLine($"Erro, nenhuma fatura encontrada!");
                        }

                        
                        break;
                    case "3":
                        Console.WriteLine("\n-=-= Relatório: Top 10 Maiores Faturas -=-=");

                        try
                        {
                            var top10Faturas = await faturaRelatorioService.ObterTop10FaturasAsync();
                            foreach (var fatura in top10Faturas)
                            {
                                Console.WriteLine($"- Fatura ID: {fatura.FaturaId}, Cliente: {fatura.Cliente?.Nome}, Total: {fatura.Total:C}");
                            }
                        }
                        catch (KeyNotFoundException ex)
                        {
                            Console.WriteLine($"Erro, nenhuma fatura encontrada!");
                        }
                        
                        
                        break;
                    case "4":
                        Console.WriteLine("\n-=-= Relatório: Top 10 Maiores Itens -=-=");
                        try
                        {
                            var top10ItensFaturas = await faturaRelatorioService.ObterTop10ItensAsync();
                            foreach (var fatura in top10ItensFaturas)
                            {
                                Console.WriteLine($"    - ID: {fatura.FaturaItemId} Ordem: {fatura.Ordem}, Desc: {fatura.Descricao}, Valor: {fatura.Valor:C}");
                            }
                        }
                        catch (KeyNotFoundException ex)
                        {
                            Console.WriteLine($"Erro, nenhuma fatura encontrada!");
                        }
                       
                        break;
                    case "0":
                        reporting = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao gerar relatório: {ex.Message}");
            }
            if (reporting)
            {
                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    private static async Task InserirDadosDeTeste(IClienteRepository clienteRepo,IFaturaRepository faturaRepo, IFaturaItemRepository itemRepo)
    {
        var clientesExistem = await clienteRepo.GetAllAsync();
        if (clientesExistem.Any())
        {
            Console.WriteLine("Dados de teste já foram inseridos anteriormente.");
            return;
        }

        var cliente1 = new Cliente { Nome = "Maria Silva" };
        var cliente2 = new Cliente { Nome = "João Oliveira" };
        var cliente3 = new Cliente { Nome = "Carla Souza" };

        await clienteRepo.AddAsync(cliente1);
        await clienteRepo.AddAsync(cliente2);
        await clienteRepo.AddAsync(cliente3);

        var clientes = new List<Cliente> { cliente1, cliente2, cliente3 };

        int faturaIdSeed = 1;
        int ordemSeed = 10;
        Random random = new Random();

        for (int i = 0; i < 15; i++)
        {
            var cliente = clientes[i % 3]; 
            var data = new DateTime(2025, (i % 12) + 1, (i % 28) + 1); 
            var itens = new List<FaturaItem>();

            int qtdItens = random.Next(1, 4); 
            for (int j = 0; j < qtdItens; j++)
            {
                itens.Add(new FaturaItem {
                    Ordem = ordemSeed,
                    Valor = random.Next(50, 1500), 
                    Descricao = $"Item {ordemSeed}"
                });
                ordemSeed += 10;
            }

            var fatura = new Fatura {
                Cliente = cliente,
                Data = data,
                FaturaItem = itens
            };

            await faturaRepo.AddAsync(fatura);
        }

        Console.WriteLine("Dados de teste inseridos com sucesso. Pressione qualquer tecla para voltar ao menu.");
        BancoPopulado = true;
        Console.ReadKey();
    }
}