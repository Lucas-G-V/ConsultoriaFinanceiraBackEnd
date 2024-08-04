# Sistema de Gestão de Portfólio de Investimentos

## Introdução

Este sistema é uma aplicação de gestão de portfólio de investimentos que utiliza uma arquitetura de microsserviços, implementando CQRS (Command Query Responsibility Segregation) e padrões de design modernos. A aplicação é composta por múltiplas APIs que gerenciam diferentes aspectos do sistema financeiro, como transações e produtos de renda fixa. Utilizamos o Entity Framework Core para acesso ao banco de dados e integrações com RabbitMQ e Redis para mensageria e cache.

## Estrutura do Projeto

A aplicação é dividida em múltiplos microsserviços, cada um responsável por uma parte específica do sistema. Aqui estão alguns dos principais componentes:

- **API de Autenticação**: Gerencia autenticação e autorização.
- **API de Transações**: Gerencia transações financeiras.
- **API de Produtos de Renda Fixa**: Gerencia produtos financeiros de renda fixa.
- **Mensageria**: Utiliza RabbitMQ para comunicação entre microsserviços.
- **Cache**: Utiliza Redis para caching.

## Requisitos

- .NET 7.0 ou superior
- Docker (para executar containers de Redis e RabbitMQ)

## Configuração do Ambiente

### 1. Configuração do Banco de Dados

Cada API utiliza o Entity Framework Core para interagir com o banco de dados. Após clonar o repositório e abrir a solução no Visual Studio ou em um editor de sua escolha, você precisa atualizar o banco de dados. 

Para isso, abra o **Console do Gerenciador de Pacotes** no Visual Studio e execute o comando:

```powershell
Update-Database
```

Use uma ConnectionString a sua escolha, os bancos estão separados por API, seguindo o Padrão de Microsserviços

### 2. Execute os containers do RabbitMQ e do Redis

Configure a connectionString no appSettings.json dos dois. Por padrão, utilizei as seguintes imagens do DockerHUB:
Para o redis, execute o comando:
```powershell
docker run --name meu-redis -d redis
```
Para o RabbitMQ, execute o comando:
```powershell
docker run -d --name rabbitmq-management -p 5672:5672 -p 15672:15672 rabbitmq:management
```
### 3. Execute todas as APIs

Utilizando Visual Studio, clique para inicializar mais de um projeto e selecione todos.

### 4. Testes

Para testar, utilize o comando:

```powershell
dotnet test
```

