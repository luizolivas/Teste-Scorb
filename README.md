# Teste

Projeto com arquitetura em camadas utilizando ASP.NET Core, Razor Pages, e Entity Framework Core. O sistema realiza o cadastro de clientes, faturas e itens de fatura, e permite a geração de relatórios.


##  Estrutura do Projeto
````
TesteDengineCore/
- TesteDengine.Domain/ # Entidades e interfaces de repositório

- TesteDengine.Application/ # Serviços de negócio e DTOs

- TesteDengine.Infrastructure/ # Persistência de dados (EF Core)

- TesteDengine.WebMVC/ # Front-end MVC com relatórios e CRUDs

- TesteDengine.Testes/ # Testes unitários dos serviços

- TesteDengineCore/ # Projeto Console (integrações / ferramentas)

- TesteDengineCore.sln # Solução principal
````

##  Como rodar o projeto

> Pré-requisitos: [.NET 8 SDK]


## 1. Clonar o repositório

```bash
git clone https://github.com/luizolivas/Teste-Scorb.git
cd Teste-Scorb
```

## 2. Restaurar pacotes

```bash
dotnet restore
```

## 3 Executar o projeto Web (ASP.NET)

```bash
cd TesteDengine.WebMVC
dotnet run
```

Acesse: https://localhost:{porta}


## 3 Executar o projeto Console 

```bash
cd .\TesteDengineCore\
dotnet run
```

## Funcionalidades

- CRUD de Clientes
- CRUD de Faturas e Itens de Fatura
- Relatórios:
   - Total por Cliente
   - Total por Mês/Ano
   - Top Faturas
   - Top Itens

(Obs: Projeto do Console conta com uma função de seed com dados para teste)

##  Arquitetura

Camadas:
Domain: entidades e contratos (interfaces)

Application: regras de negócio e DTOs

Infrastructure: acesso a dados com EF Core

WebMVC / API: apresentação (frontend)

Testes: testes unitários com xUnit

Console: testes manuais


##  Testes

Execute os testes com:

```bash
cd TesteDengine.Testes
dotnet test
```

## Tecnologias Utilizadas

🔧 Tecnologias Utilizadas

- ASP.NET Core / Razor Pages
- Entity Framework Core (InMemory)
- xUnit (testes)
- Bootstrap (interface)
