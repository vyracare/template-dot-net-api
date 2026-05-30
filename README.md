[repo-generic] (.NET 8) - MongoDB + AWS Lambda
-----------------------------------------------

Descricao:
  - [description-generic]

Recursos iniciais:
  - Projeto .NET 8 preparado para AWS Lambda
  - MongoDB configurado com database `[database-generic]`
  - Controller inicial para a collection `[table-generic]`
  - Rotas base publicadas em `/api/[table-route-generic]`
  - Swagger habilitado em `/swagger/index.html`
  - CORS configurado por default para nao bloquear integracoes
  - JWT obrigatorio em todos os endpoints da API
  - Branch `develop` criada automaticamente a partir da `main`
  - Protecao basica habilitada em `main` e `develop`
  - Configuracao opcional de MFE consumidor em `.vyracare/mfe-consumer.json`

Integracao opcional com MFE:
  - Informe o repositório do MFE no issue form apenas quando a API precisar atualizar automaticamente a URL consumida
  - O template grava essa relacao no arquivo `.vyracare/mfe-consumer.json`
  - A esteira generica usa esse arquivo para descobrir qual repositório de frontend deve ser atualizado

Setup local:
  - Install .NET 8 SDK
  - Configure `dotnet user-secrets` ou use as env vars `MONGO_URI` e `JWT_KEY`
  - Ajuste `Cors:AllowedOrigins` caso queira restringir origens
  - `dotnet restore`
  - `dotnet build`
  - `dotnet run`

To publish:
  - `dotnet publish -c Release -o ./publish`
  - Por padrao o template aponta para os secrets AWS `vyracare/shared/mongo` e `vyracare/shared/jwt-signing`
  - O `appsettings.json` do template fica sem credenciais reais
  - Opcionalmente configure `CORS_ALLOWED_ORIGINS` para sobrescrever os domínios permitidos
  - Opcionalmente configure `JWT_ISSUER` e `JWT_AUDIENCE` para sobrescrever os valores versionados
