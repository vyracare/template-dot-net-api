[repo-generic] (.NET 8) - MongoDB + AWS Lambda
-------------------------------------

Descricao:
  - [description-generic]

Recursos iniciais:
  - Projeto .NET 8 preparado para AWS Lambda
  - MongoDB configurado com database `[database-generic]`
  - Controller inicial para a collection `[table-generic]`
  - Rotas base publicadas em `/api/[table-route-generic]`

Setup local:
  - Install .NET 8 SDK
  - Configure a MongoDB cluster and set `MONGO_URI` env var or update `appsettings.json`
  - `dotnet restore`
  - `dotnet build`
  - `dotnet run`

To publish:
  - `dotnet publish -c Release -o ./publish`
