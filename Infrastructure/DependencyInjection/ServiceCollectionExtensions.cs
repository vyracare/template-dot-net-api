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

public static class ServiceCollectionExtensions
{
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
