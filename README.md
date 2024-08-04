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

### Usuários

#### Registrar Novo Usuário Administrador

**Método**: POST /api/Usuarios/ContaAdmin

**Descrição**: Para utilizar o sistema, acompanhar e criar Produtos Financeiros crie um usuário Administrador.

**Requisitos de Autorização**: Como é uma aplicação teste, a criação de usuários é livre.

**Parâmetros**:
- **Request Body**: 
```json
{
  "login": "email",
  "senha": "sua senha"
}
```
**Resposta**:
- **204 No Content**: Se o usuário for registrado com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

#### Registrar Novo Usuário Cliente

**Método**: POST /api/Usuarios/ContaCliente

**Descrição**: Para utilizar o sistema, acompanhar e criar Transações Financeiras, crie um Cliente, no seu processo de criação, ele comunicará a API ContaCliente e adicionará a ela um Saldo Disponível de 0. Ele verifica se há uma conta com o mesmo CPF, então cuidado ao cadastrar mais de um usuário, o login também deve ser único, incluindo a base de Administradores.

**Requisitos de Autorização**: Como é uma aplicação teste, a criação de clientes é livre.

**Parâmetros**:
- **Request Body**:
```json
{
  "login": "email",
  "senha": "string",
  "nomeCompleto": "string",
  "cpf": "string",
  "telefoneCelular": "string"
}
```
**Resposta**:
- **204 No Content**: Se o usuário for registrado com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

#### Login

**Método**: POST /api/Login

**Descrição**: Realiza o login de um usuário e retorna um JWT se as credenciais forem válidas.

**Parâmetros**:
- **Request Body**: 
```json
{
  "login": "email",
  "senha": "sua senha"
}
```

**Resposta**:
- **200 OK**: Retorna um JWT se o login for bem-sucedido.
- **400 Bad Request**: Se houver erros de validação.
- **401 Unauthorized**: Se as credenciais estiverem incorretas ou o usuário estiver bloqueado.

---
#### Conta do Cliente

**Método**: GET /api/ContaCliente/GetSaldoAtual

**Descrição**: Verifica as informações da conta do usuário resumidamente. Informações como número de telefone, cpf, saldo da conta e total investido.

**Resposta**:
- **200 OK**: Retorna as informações do usuário.
- **400 Bad Request**: Se houver erros de validação.
- **401 Unauthorized**: Se as credenciais estiverem incorretas ou o usuário estiver bloqueado (Use um token de um cliente para fazer a solicitação, o id do cliente é baseado no Token).

---

### Produtos de Renda Fixa

#### Criar Produto de Renda Fixa

**Método**: POST /api/RendaFixa

**Descrição**: Cria um novo produto de renda fixa, apenas produtos baseados em cotas foram testados. A implementação de produtos não baseados em cotas ainda está sendo desenvolvida. Por isso há algumas restrições na hora do post que quando tivesse mais gamas implementadas de renda fixa, não teriam.

**Requisitos de Autorização**: Precisa ser um usuário administrador.

**Parâmetros**:
- **Request Body**:
```json
{
  "nome": "string",
  "valorMinimo": 0,
  "valorUnitario": 0,
  "baseadoEmCotas": true,
  "dataVencimento": "2024-08-04T20:11:11.109Z",
  "tipoTaxa": 1 (1 -> Percentual), (2 -> Fixa)
  "taxaAnual": 0,
  "taxaAdicional": 0,
  "indexador": 1 (1 -> CDI, 2 -> IPCA, 3 -> SELIC, 4 -> Outros),
  "frequencia": 1,
  "quantidadeCotasInicial": 0,
  "quantidadeCotasDisponivel": 0,
  "emailAdministrador": "string"
}
```

**Resposta**:
- **200 No Content**: Se o produto de renda fixa for criado com sucesso, preencha o valor unitário e o valor mínimo total da compra, a data de Vencimento deve ser posterior a atual.
- **400 Bad Request**: Se houver erros de validação.

---

#### Atualizar Quantidade de Cotas Disponível

**Método**: PUT /api/RendaFixa/AlteraProdutoFinanceiro

**Descrição**: Atualiza as informações de um produto existente, desde seu valor atual no mercado, o valor mínimo de compra e a quantidade de cotas disponíveis.

**Requisitos de Autorização**: Deve ser um administrador.

**Parâmetros**:
- **Request Body**: 
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "quantidadeCotasDisponivel": 0,
  "nome": "string",
  "valorMinimo": 0,
  "valorUnitario": 0,
  "emailAdministrador": "string"
}
```

**Resposta**:
- **200 No Content**: Se for atualizado com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

#### Obter Todos os Produtos de Renda Fixa

**Método**: GET /api/RendaFixa/GetAll

**Descrição**: Recupera uma lista de todos os produtos de renda fixa.

**Requisitos de Autorização**:Precisa ser administrador.

**Resposta**:
- **200 OK**: Retorna uma lista de produtos de renda fixa.
- **400 Bad Request**: Se houver um erro na requisição.

---

#### Obter Todos os Produtos de Renda Fixa Válidos para um Cliente

**Método**: GET /api/RendaFixa/GetAllForClientes

**Descrição**: Recupera uma lista de todos os produtos de renda fixa.

**Requisitos de Autorização**: Autenticado.

**Resposta**:
- **200 OK**: Retorna uma lista de produtos de renda fixa.
- **400 Bad Request**: Se houver um erro na requisição.

---

#### Obter o extrado de um produto

**Método**: GET /api/RendaFixa/GetById/{id} sendo {id} o id do produto

**Descrição**: Recupera o produto com uma lista de histório dos seus valores.

**Requisitos de Autorização**: Autenticado.

**Resposta**:
- **200 OK**: Retorna uma lista de produtos de renda fixa.
- **400 Bad Request**: Se houver um erro na requisição.

---




### Transações

#### Fazer Depósito

**Método**: POST /api/Transacao/Deposito

**Descrição**: Cria uma nova transação. O depósito será feito e atualizará o saldo do cliente caso queira consultar na API ContaCliente no EndPoint: /api/ContaCliente/GetSaldoAtual

**Requisitos de Autorização**: Deve ser um cliente.

**Parâmetros**:
- **Request Body**:
```json
{
  "valorTotal": 0
}
```
**Resposta**:
- **200 No Content**: Se a transação for criada com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

#### Fazer Saque

**Método**: POST /api/Transacao/Saque

**Descrição**: Cria uma nova transação. O saque será feito e atualizará o saldo do cliente caso queira consultar na API ContaCliente no EndPoint: /api/ContaCliente/GetSaldoAtual. Caso o usuário não tenha um saldo disponível em conta, a transação será invalidada. Lembrando que no modelo estabelecido, o saldo disponível dele se dá pela (quantidade de depósitos feitas + vendas de produtos financeiros) - (a quantidade de saques + quantidade de compras de produtos financeiros).

**Requisitos de Autorização**: Deve ser um cliente.

**Parâmetros**:
- **Request Body**:
```json
{
  "valorTotal": 0
}
```
**Resposta**:
- **200 No Content**: Se a transação for criada com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

#### Fazer Compra de ProdutoFinanceiro (Apenas renda fixa com cotas disponíveis por enquanto)

**Método**: POST /api/Transacao/Compra

**Descrição**: Cria uma nova transação. A compra será feito e atualizará o saldo do cliente caso queira consultar na API ContaCliente no EndPoint: /api/ContaCliente/GetSaldoAtual. Caso o usuário não tenha um saldo disponível em conta, a transação será invalidada. Lembrando que no modelo estabelecido, o saldo disponível dele se dá pela (quantidade de depósitos feitas + vendas de produtos financeiros) - (a quantidade de saques + quantidade de compras de produtos financeiros). Ele também válida se no momento da transação há quantidades disponíveis suficientes do produto financeiro e se o valor mínimo foi estabelecido. O nome do produto é necessário apenas como um fator a mais de segurança.

**Requisitos de Autorização**: Deve ser um cliente.

**Parâmetros**:
- **Request Body**:
```json
{
  "produtoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "nomeProduto": "string",
  "quantidade": 0,
  "valorUnitario": 0
}
```
**Resposta**:
- **200 No Content**: Se a transação for criada com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

#### Fazer Venda de ProdutoFinanceiro (Apenas renda fixa com cotas disponíveis por enquanto)

**Método**: POST /api/Transacao/Venda

**Descrição**: Cria uma nova transação. A venda será feita e atualizará o saldo do cliente, caso queira consultar na API ContaCliente no EndPoint: /api/ContaCliente/GetSaldoAtual. Caso o usuário não tenha as ações determinadas, a transação será invalidada. (Futura implementação garantirá que o produto financeiro pode ser vendido e quais regras do retorno, por enquanto ele adiciona mais quantidades disponíveis do produto financeiro e atualiza o totalInvestido da conta do cliente, voltando para o saldo disponível ao finalizar a transação).

**Requisitos de Autorização**: Deve ser um cliente.

**Parâmetros**:
- **Request Body**:
```json
{
  "produtoId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "nomeProduto": "string",
  "quantidade": 0,
  "valorUnitario": 0
}
```
**Resposta**:
- **200 No Content**: Se a transação for criada com sucesso.
- **400 Bad Request**: Se houver erros de validação.

---

#### Consultar o histório de transações do cliente

**Método**: GET /api/Transacao/GetExtratoCliente

**Descrição**: Verifica as transações do cliente e traz em ordem cronológica. No caso, todas virão como aprovadas, a implementação do status é alterado no próprio fluxo, porém caso fosse necessário um menor aclopamento entre os produtos financeiros e as transações, seria possível emitir um evento de inicio transação, salvar no banco como em andamento e depois de validado, a api do produto financeiro emite outro evento e aprova ou cancela uma transação. O modelo está implementado para aceitar esse tipo de ajuste. Por questões de consistência nos dados no momento do início da transação, foi definido que a definição se daria no momento de início da transação.

**Requisitos de Autorização**: Deve ser um cliente.



**Resposta**:
- **200 OK**: Retorna os dados do cliente.
- **400 Bad Request**: Se houver erros de validação.

---

#### Consultar o histório de transações do cliente

**Método**: GET /api/Transacao/GetExtratoClienteAdmin/{idCliente}

**Descrição**: Verifica as transações do cliente e traz em ordem cronológica. No caso, todas virão como aprovadas, a implementação do status é alterado no próprio fluxo, porém caso fosse necessário um menor aclopamento entre os produtos financeiros e as transações, seria possível emitir um evento de inicio transação, salvar no banco como em andamento e depois de validado, a api do produto financeiro emite outro evento e aprova ou cancela uma transação. O modelo está implementado para aceitar esse tipo de ajuste. Por questões de consistência nos dados no momento do início da transação, foi definido que a definição se daria no momento de início da transação.

**Requisitos de Autorização**: Deve ser um administrador.


**Resposta**:
- **200 OK**: Retorna os dados do cliente.
- **400 Bad Request**: Se houver erros de validação.

---



## Notas Adicionais

- **CustomResponse**: Utilizado para padronizar as respostas da API com base nos resultados das operações.
- **ClaimsAuthorize**: Utilizado para garantir que os usuários tenham as permissões necessárias para acessar os endpoints.

Este é um resumo dos principais endpoints disponíveis na aplicação. Para detalhes adicionais sobre como configurar e utilizar a API, consulte a documentação completa ou o código fonte.
## Próximos Passos

- Realizar mais testes de integração, por enquanto só a API de autenticação foi testada
- Realizar mais testes de unidade, cobrindo a maior parte da aplicação
- Adicionar os outros produtos financeiros
- Especializar ainda mais a classe RendaFixa para aceitar diferentes parâmetros da RendaFixa e validar essas regras na venda e compra dos produtos
- Adicionar o serviço de email em uma WorkerService, sendo utilizado de forma desaclopada da RendaFixa


