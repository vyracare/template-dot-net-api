# template-dot-net-api

Template base para criar novas APIs .NET 8 da Vyracare com arquitetura padronizada, seguranca alinhada com AWS Secrets Manager e camada inicial de testes unitarios.

## Estrutura gerada

O template agora nasce em um modelo pragmatico de `vertical slice`.

- `Features/<Recurso>/Create`
  Caso de uso de criacao.
- `Features/<Recurso>/List`
  Caso de uso de listagem.
- `Features/<Recurso>/GetById`
  Caso de uso de consulta individual.
- `Features/<Recurso>/Shared`
  Entidade de dominio e contrato de repositorio.
- `Common`
  Tipos compartilhados de configuracao, resultado, abstração de tempo e extensoes HTTP.
- `Infrastructure/Persistence`
  Adapter MongoDB.
- `Infrastructure/DependencyInjection`
  Bootstrap de handlers, repositorios e banco.
- `<Assembly>.Tests`
  Projeto de testes unitarios pronto para evolucao.

## O que o template entrega

- API .NET 8 preparada para AWS Lambda + HTTP API.
- JWT obrigatorio por default.
- Swagger habilitado.
- CORS configuravel.
- Integracao com AWS Secrets Manager.
- Estrutura pronta para sincronizar MFE consumidor quando configurado.
- Projeto de testes unitarios com exemplos para handlers.

## Seguranca

O `appsettings.json` do template nao carrega credenciais reais.

Secrets padrao utilizados:
- `vyracare/shared/mongo`
- `vyracare/shared/jwt-signing`

Fallbacks suportados:
- `MONGO_URI`
- `JWT_KEY`
- `JWT_ISSUER`
- `JWT_AUDIENCE`
- `CORS_ALLOWED_ORIGINS`

## Rename do template

O script [rename-dotnet-project.sh](C:/Users/lenin/OneDrive/Desktop/GitHub/Vyracare/template-dot-net-api/rename-dotnet-project.sh) agora renomeia:

- arquivos
- diretorios
- projeto principal
- projeto de testes
- nomes internos baseados nos placeholders

Isso permite gerar o esqueleto novo sem manter nomes genericos no repositório criado.

## Testes unitarios

O template cria um projeto `<Assembly>.Tests` com cobertura inicial para:

- handler de criacao
- handler de consulta por id

Comando esperado no projeto gerado:

```bash
dotnet test
```

## Execucao local

```bash
dotnet restore
dotnet build
dotnet run
```

## Deploy

As APIs geradas por este template usam a esteira reutilizavel do `vyracare-infra-pipes-dot-net`, incluindo:

- publish em Lambda
- rotas no API Gateway
- Swagger
- sincronizacao opcional do MFE consumidor
