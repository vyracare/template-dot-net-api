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

Setup local:
  - Install .NET 8 SDK
  - Configure a MongoDB cluster and set `MONGO_URI` env var or update `appsettings.json`
  - Ajuste `Cors:AllowedOrigins` caso queira restringir origens
  - `dotnet restore`
  - `dotnet build`
  - `dotnet run`

To publish:
  - `dotnet publish -c Release -o ./publish`
