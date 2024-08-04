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

Para isso, abra o **Console do Gerenciador de Pacotes** no Visual Studio, selecione a API referente e execute o comando:

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

## Modo de Uso e Documentação da API

### Transações

#### Criar Transação

**Método**: POST /api/transacoes

**Descrição**: Cria uma nova transação. A transação será criada com o status `Pendente` e a data atual.

**Requisitos de Autorização**: Deve ter a permissão `Escrever` para a entidade `Transacao`.

**Parâmetros**:
- **Request Body**: `CreateTransacaoRequest` - Objeto contendo os dados necessários para criar uma nova transação.

**Resposta**:
- **200 No Content**: Se a transação for criada com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

### Produtos de Renda Fixa

#### Criar Produto de Renda Fixa

**Método**: POST /api/rendaFixa

**Descrição**: Cria um novo produto de renda fixa.

**Requisitos de Autorização**: Deve ter a permissão `Escrever` para a entidade `RendaFixa`.

**Parâmetros**:
- **Request Body**: `CreateRendaFixaRequest` - Objeto contendo os dados necessários para criar um novo produto de renda fixa.

**Resposta**:
- **200 No Content**: Se o produto de renda fixa for criado com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

#### Atualizar Quantidade de Cotas Disponível

**Método**: PUT /api/rendaFixa/AlteraQuantidadeCotasDisponivel

**Descrição**: Atualiza a quantidade de cotas disponíveis de um produto de renda fixa existente.

**Requisitos de Autorização**: Deve ter a permissão `Editar` para a entidade `RendaFixa`.

**Parâmetros**:
- **Request Body**: `UpdateRendaFixaRequest` - Objeto contendo os dados necessários para atualizar a quantidade de cotas de um produto de renda fixa.

**Resposta**:
- **200 No Content**: Se a quantidade de cotas for atualizada com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

#### Obter Todos os Produtos de Renda Fixa

**Método**: GET /api/rendaFixa/GetAll

**Descrição**: Recupera uma lista de todos os produtos de renda fixa.

**Requisitos de Autorização**: Deve ter a permissão `Ler` para a entidade `RendaFixa`.

**Resposta**:
- **200 OK**: Retorna uma lista de produtos de renda fixa.
- **400 Bad Request**: Se houver um erro na requisição.

---

### Autenticação e Registro de Usuário

#### Login

**Método**: POST /api/login

**Descrição**: Realiza o login de um usuário e retorna um JWT se as credenciais forem válidas.

**Parâmetros**:
- **Request Body**: `LoginRequest` - Objeto contendo o login e senha do usuário.

**Resposta**:
- **200 OK**: Retorna um JWT se o login for bem-sucedido.
- **400 Bad Request**: Se houver erros de validação.
- **401 Unauthorized**: Se as credenciais estiverem incorretas ou o usuário estiver bloqueado.

---

#### Registrar Novo Usuário Administrador

**Método**: POST /api/ContaAdmin

**Descrição**: Registra um novo usuário administrador no sistema.

**Requisitos de Autorização**: Nenhum (requer autenticação do administrador).

**Parâmetros**:
- **Request Body**: `CreateUserAdminRequest` - Objeto contendo os dados necessários para criar um novo usuário administrador.

**Resposta**:
- **200 No Content**: Se o usuário for registrado com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

## Notas Adicionais

- **CustomResponse**: Utilizado para padronizar as respostas da API com base nos resultados das operações.
- **ClaimsAuthorize**: Utilizado para garantir que os usuários tenham as permissões necessárias para acessar os endpoints.

Este é um resumo dos principais endpoints disponíveis na aplicação. Para detalhes adicionais sobre como configurar e utilizar a API, consulte a documentação completa ou o código fonte.

