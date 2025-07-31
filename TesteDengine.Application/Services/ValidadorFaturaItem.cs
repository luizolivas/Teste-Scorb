using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteDengine.Application.Services
{
    public class ValidadorFaturaItem
    {
        public void Validar(FaturaItem item, ICollection<FaturaItem> itensExistentes)
        {
            ValidarDescricao(item.Descricao);
            ValidarValor(item.Valor);
            ValidarOrdem(item.Ordem, itensExistentes);
            ValidarOrdemSemBuracos(item.Ordem, itensExistentes);
            ValidarConfirmacaoDeValor(item.Valor);
        }

        private void ValidarDescricao(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao))
                throw new Exception("Descrição é obrigatória.");

            if (descricao.Length > 20)
                throw new Exception("Descrição deve ter no máximo 20 caracteres.");
        }

        private void ValidarValor(double valor)
        {
            if (valor <= 0)
                throw new Exception("O valor deve ser positivo.");
        }

        private void ValidarOrdem(int ordem, ICollection<FaturaItem> itensExistentes)
        {
            if (ordem % 10 != 0)
                throw new Exception("A ordem deve ser múltiplo de 10.");

            if (itensExistentes.Any(i => i.Ordem == ordem))
                throw new Exception("A ordem deve ser única dentro da fatura.");
        }

        private void ValidarOrdemSemBuracos(int novaOrdem, ICollection<FaturaItem> itensExistentes)
        {
            var ordens = itensExistentes.Select(i => i.Ordem).ToList();
            ordens.Add(novaOrdem);
            ordens.Sort();

            for (int i = 1; i < ordens.Count; i++)
            {
                if (ordens[i] - ordens[i - 1] != 10)
                    throw new Exception("A sequência da ordem deve ser contínua e múltipla de 10.");
            }
        }

        private void ValidarConfirmacaoDeValor(double valor)
        {
            if (valor > 1000)
            {
                Console.WriteLine("Valor acima de 1000. Confirmar valor? (s/n):");
                if (Console.ReadLine()?.ToLower() != "s")
                    throw new Exception("Valor não confirmado.");
            }
        }
    }
}
