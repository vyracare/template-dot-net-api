using Amazon.Lambda.AspNetCoreServer;
using Amazon.Lambda.AspNetCoreServer.Hosting;
using MongoDB.Driver;
using [assembly-generic].Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var mongoUri = configuration["Mongo:ConnectionString"] ?? Environment.GetEnvironmentVariable("MONGO_URI") ?? "mongodb://localhost:27017";
var mongoDatabase = configuration["Mongo:Database"] ?? "[database-generic]";

builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoUri));
builder.Services.AddScoped(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(mongoDatabase));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

builder.Services.AddScoped<[resource-generic]Service>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok", api = "[repo-generic]" }));

app.Run();
