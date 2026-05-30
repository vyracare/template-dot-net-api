using Microsoft.Extensions.Options;
using MongoDB.Driver;
using [assembly-generic].Common.Configuration;
using [assembly-generic].Common.Time;
using [assembly-generic].Features.[resource-generic].Create;
using [assembly-generic].Features.[resource-generic].GetById;
using [assembly-generic].Features.[resource-generic].List;
using [assembly-generic].Features.[resource-generic].Shared.Ports;
using [assembly-generic].Infrastructure.Persistence;
using [assembly-generic].Infrastructure.Time;

namespace [assembly-generic].Infrastructure.DependencyInjection;

/// <summary>
/// Centraliza extens?es reutiliz?veis usadas pela aplica??o.
/// </summary>
public static class ServiceCollectionExtensions
{
/// <summary>
/// Registra os servi?os necess?rios para conectar a aplica??o ao MongoDB.
/// </summary>
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        services.AddSingleton<IMongoClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
            return new MongoClient(options.ConnectionString);
        });

        services.AddScoped(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
            return sp.GetRequiredService<IMongoClient>().GetDatabase(options.Database);
        });

        return services;
    }

/// <summary>
/// Registra os handlers, portas e servi?os centrais da aplica??o no container de depend?ncias.
/// </summary>
    public static IServiceCollection AddApplicationCore(this IServiceCollection services)
    {
        services.AddSingleton<IClock, SystemClock>();
        services.AddScoped<I[resource-generic]Repository, Mongo[resource-generic]Repository>();
        services.AddScoped<Create[resource-generic]Handler>();
        services.AddScoped<Get[resource-generic]ByIdHandler>();
        services.AddScoped<List[resource-generic]Handler>();
        return services;
    }
}
