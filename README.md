# template-dot-net-api

## Visao geral

Este repositorio e o template base usado para gerar novas APIs .NET da Vyracare.

Ele ja nasce com:

- arquitetura em `vertical slice`
- JWT
- MongoDB
- AWS Lambda
- Swagger
- integracao com AWS Systems Manager Parameter Store
- projeto de testes unitarios
- automacao de rename para adaptar nomes de assembly, recurso e rotas

## O que acontece quando um projeto e criado a partir deste template

A automacao:

1. cria o novo repositorio
2. clona o template
3. executa o [rename-dotnet-project.sh](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/rename-dotnet-project.sh)
4. substitui placeholders como:
   - `[assembly-generic]`
   - `[resource-generic]`
   - `[table-generic]`
   - `[repo-generic]`
5. renomeia arquivos e diretorios
6. publica o projeto final no novo repositorio

Por isso este template bruto nao compila antes do rename. Ele e um scaffold parametrizado.

## Como ler este template

Leia nesta ordem:

1. [Program.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Program.cs)
2. [rename-dotnet-project.sh](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/rename-dotnet-project.sh)
3. A feature generica em `Features/[resource-generic]`
4. A porta em `Shared/Ports`
5. O adapter Mongo em `Infrastructure/Persistence`
6. O projeto de testes em `Vyracare.Api.[name-generic].Tests`

## Estrutura que toda API gerada recebe

### `Common`

Pecas compartilhadas:

- options
- resultado padrao
- abstracao de tempo
- extensoes HTTP

### `Features/<Recurso>`

Cada caso de uso do recurso fica em uma pasta propria:

- `Create`
- `List`
- `GetById`

### `Features/<Recurso>/Shared`

Contem:

- entidade de dominio
- interface de repositorio

### `Infrastructure`

Contem detalhes tecnicos:

- leitura de parametros seguros
- repositorio Mongo
- documento Mongo
- DI

### `<Assembly>.Tests`

Contem exemplos iniciais de testes unitarios para o projeto gerado.

## O que um desenvolvedor precisa entender sobre este template

- o template nao e a API final
- ele e uma matriz parametrizada
- os placeholders representam nomes que so vao existir no projeto gerado
- a estrutura criada serve como ponto de partida para novas features reais

## Como a arquitetura gerada funciona

Quando a API for criada a partir do template, o fluxo sera:

1. controller recebe a request
2. handler executa a regra
3. handler usa uma porta
4. infraestrutura implementa a porta com MongoDB
5. controller devolve a resposta HTTP

Essa estrutura foi escolhida porque:

- deixa o dominio mais claro
- facilita testes unitarios
- reduz acoplamento com detalhes tecnicos

## Seguranca e configuracao

O template ja nasce sem segredos reais versionados.

Parametros padrao:

- `vyracare/shared/mongo-prod`
- `vyracare/shared/mongo-hml`
- `vyracare/shared/mongo-dev`
- `vyracare/shared/jwt-signing-prod`
- `vyracare/shared/jwt-signing-hml`
- `vyracare/shared/jwt-signing-dev`

Fallbacks suportados:

- `MONGO_URI`
- `JWT_KEY`
- `JWT_ISSUER`
- `JWT_AUDIENCE`
- `CORS_ALLOWED_ORIGINS`

Arquivos importantes:

- [appsettings.json](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/appsettings.json)
- [ParameterStoreBootstrapper.cs](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/Infrastructure/ParameterStoreBootstrapper.cs)

## Testes unitarios no template

O template ja gera exemplos para:

- criacao do recurso
- consulta por id

Objetivo:

- mostrar o padrao esperado de teste
- facilitar que novas APIs ja nascam com cultura de teste
- evitar depender de banco real nos testes de regra

## Como adicionar novos casos de uso no template

Se no futuro a Vyracare quiser que toda nova API nasca tambem com `Update` e `Delete`, o caminho e:

1. criar novas pastas em `Features/[resource-generic]`
2. adicionar handlers, requests e portas
3. atualizar o controller
4. atualizar o repositorio Mongo
5. registrar tudo em `ServiceCollectionExtensions`
6. expandir o projeto de testes
7. garantir que o `rename-dotnet-project.sh` continua cobrindo todos os placeholders

## Observacao importante sobre build do template

Este repositorio, do jeito que esta, nao e um projeto de negocio compilavel, porque os placeholders ainda fazem parte do codigo-fonte.

O que e compilavel e o projeto gerado apos a automacao de rename.

Entao a validacao correta do template e:

1. gerar um projeto novo
2. rodar o rename
3. compilar o repositorio gerado
4. executar os testes do repositorio gerado

## Convencao de commits

Os projetos gerados a partir deste template devem seguir o padrao de commits em portugues.

Exemplos:

- `feat: adiciona endpoint de agendamento`
- `fix: corrige leitura de parametro do mongo`
- `docs: atualiza readme da api`
