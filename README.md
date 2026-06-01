# template-dot-net-api

## Visao geral

Este repositório e o template base usado para gerar novas APIs .NET da Vyracare.

Ele ja nasce com:

- arquitetura em `vertical slice`;
- JWT;
- MongoDB;
- AWS Lambda;
- Swagger;
- integracao com AWS Secrets Manager;
- projeto de testes unitarios;
- automacao de rename para adaptar nomes de assembly, recurso e rotas.

---

## O que acontece quando um projeto e criado a partir deste template

A automacao:

1. cria o novo repositório;
2. clona o template;
3. executa o [rename-dotnet-project.sh](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/rename-dotnet-project.sh);
4. substitui placeholders como:
   - `[assembly-generic]`
   - `[resource-generic]`
   - `[table-generic]`
   - `[repo-generic]`
5. renomeia arquivos e diretorios;
6. publica o projeto final no novo repositório.

Por isso este template bruto nao compila antes do rename. Ele e um scaffold parametrizado.

---

## Como ler este template

Leia nesta ordem:

1. [Program.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Program.cs)
   Mostra a espinha dorsal que toda API gerada vai receber.

2. [rename-dotnet-project.sh](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/rename-dotnet-project.sh)
   Mostra como os placeholders sao substituidos.

3. A feature generica:
   - [Create[resource-generic]Request.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Features/[resource-generic]/Create/Create[resource-generic]Request.cs)
   - [Create[resource-generic]Handler.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Features/[resource-generic]/Create/Create[resource-generic]Handler.cs)
   - [Get[resource-generic]ByIdHandler.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Features/[resource-generic]/GetById/Get[resource-generic]ByIdHandler.cs)
   - [List[resource-generic]Handler.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Features/[resource-generic]/List/List[resource-generic]Handler.cs)

4. A porta e o adapter:
   - [I[resource-generic]Repository.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Features/[resource-generic]/Shared/Ports/I[resource-generic]Repository.cs)
   - [Mongo[resource-generic]Repository.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Infrastructure/Persistence/Mongo[resource-generic]Repository.cs)

5. O projeto de testes:
   - [Vyracare.Api.[name-generic].Tests.csproj](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Vyracare.Api.[name-generic].Tests/Vyracare.Api.[name-generic].Tests.csproj)

---

## Estrutura que toda API gerada recebe

### `Common`

Pecas compartilhadas:

- options;
- resultado padrao;
- abstração de tempo;
- extensoes HTTP.

### `Features/<Recurso>`

Cada caso de uso do recurso fica em uma pasta propria:

- `Create`
- `List`
- `GetById`

### `Features/<Recurso>/Shared`

Contem:

- entidade de dominio;
- interface de repositorio.

### `Infrastructure`

Contem detalhes tecnicos:

- leitura de secrets;
- repositorio Mongo;
- documento Mongo;
- DI.

### `<Assembly>.Tests`

Contem exemplos iniciais de testes unitarios para o projeto gerado.

---

## O que um desenvolvedor junior precisa entender sobre este template

Pense assim:

- o template nao e a API final;
- ele e uma matriz parametrizada;
- os placeholders representam nomes que so vao existir no projeto gerado;
- a estrutura criada serve como ponto de partida para novas features reais.

---

## Como a arquitetura gerada funciona

Quando a API for criada a partir do template, o fluxo sera:

1. controller recebe a request;
2. handler executa a regra;
3. handler usa uma porta;
4. infraestrutura implementa a porta com MongoDB;
5. controller devolve a resposta HTTP.

Essa estrutura foi escolhida porque:

- deixa o dominio mais claro;
- facilita testes unitarios;
- reduz acoplamento com detalhes tecnicos.

---

## Seguranca e configuracao

O template ja nasce sem segredos reais versionados.

Secrets padrao:

- `vyracare/shared/mongo-prod`
- `vyracare/shared/mongo-dev`
- `vyracare/shared/jwt-signing-prod`
- `vyracare/shared/jwt-signing-dev`

Fallbacks suportados:

- `MONGO_URI`
- `JWT_KEY`
- `JWT_ISSUER`
- `JWT_AUDIENCE`
- `CORS_ALLOWED_ORIGINS`

Arquivos importantes:

- [appsettings.json](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/appsettings.json)
- [SecretsManagerBootstrapper.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Infrastructure/SecretsManagerBootstrapper.cs)

---

## Testes unitarios no template

O template ja gera exemplos para:

- criacao do recurso;
- consulta por id.

Objetivo:

- mostrar o padrao esperado de teste;
- facilitar que novas APIs ja nascam com cultura de teste;
- evitar depender de banco real nos testes de regra.

Como isso deve ser usado no projeto gerado:

1. adaptar os testes ao dominio real;
2. criar fakes para portas;
3. cobrir sucesso e falha;
4. expandir a suite conforme novas features surgirem.

---

## Como adicionar novos casos de uso no template

Se futuramente a Vyracare quiser que toda nova API nasca tambem com `Update` e `Delete`, o caminho e:

1. criar novas pastas em `Features/[resource-generic]`;
2. adicionar handlers, requests e portas;
3. atualizar o controller;
4. atualizar o repositorio Mongo;
5. registrar tudo em `ServiceCollectionExtensions`;
6. expandir o projeto de testes;
7. garantir que o `rename-dotnet-project.sh` continua cobrindo todos os placeholders.

---

## Observacao importante sobre build do template

Este repositório, do jeito que esta, nao e um projeto “de negocio” compilavel, porque os placeholders ainda fazem parte do codigo-fonte.

O que e compilavel e o projeto gerado apos a automacao de rename.

Entao a validacao correta do template e:

1. gerar um projeto novo;
2. rodar o rename;
3. compilar o repositório gerado;
4. executar os testes do repositório gerado.
