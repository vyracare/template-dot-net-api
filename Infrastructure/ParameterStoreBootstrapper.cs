using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace [assembly-generic].Infrastructure;

/// <summary>
/// Aplica os parametros externos necessarios durante a inicializacao da aplicacao.
/// </summary>
public static class ParameterStoreBootstrapper
{
    /// <summary>
    /// Aplica os valores externos necessarios antes da inicializacao completa da aplicacao.
    /// </summary>
    public static async Task ApplyAsync(ConfigurationManager configuration)
    {
        var overrides = new Dictionary<string, string?>();

        await TryAddParameterValueAsync(
            configuration,
            overrides,
            parameterNameConfigKeys: ["Parameters:MongoParameterName", "Secrets:MongoSecretName"],
            parameterNameEnvironmentVariables: ["MONGO_PARAMETER_NAME", "MONGO_SECRET_NAME"],
            fallbackEnvironmentVariable: "MONGO_URI",
            targetConfigurationKey: "Mongo:ConnectionString",
            parameterPropertyName: "ConnectionString");

        await TryAddParameterValueAsync(
            configuration,
            overrides,
            parameterNameConfigKeys: ["Parameters:JwtParameterName", "Secrets:JwtSecretName"],
            parameterNameEnvironmentVariables: ["JWT_PARAMETER_NAME", "JWT_SECRET_NAME"],
            fallbackEnvironmentVariable: "JWT_KEY",
            targetConfigurationKey: "Jwt:Key",
            parameterPropertyName: "Key");

        if (overrides.Count > 0)
        {
            configuration.AddInMemoryCollection(overrides);
        }
    }

    private static async Task TryAddParameterValueAsync(
        IConfiguration configuration,
        IDictionary<string, string?> overrides,
        IReadOnlyList<string> parameterNameConfigKeys,
        IReadOnlyList<string> parameterNameEnvironmentVariables,
        string fallbackEnvironmentVariable,
        string targetConfigurationKey,
        string parameterPropertyName)
    {
        if (!string.IsNullOrWhiteSpace(configuration[targetConfigurationKey]) ||
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(fallbackEnvironmentVariable)))
        {
            return;
        }

        var parameterName = ResolveParameterName(configuration, parameterNameConfigKeys, parameterNameEnvironmentVariables);
        if (string.IsNullOrWhiteSpace(parameterName))
        {
            return;
        }

        try
        {
            using var client = new AmazonSimpleSystemsManagementClient();
            var response = await client.GetParameterAsync(new GetParameterRequest
            {
                Name = parameterName,
                WithDecryption = true
            });

            if (string.IsNullOrWhiteSpace(response.Parameter?.Value))
            {
                return;
            }

            overrides[targetConfigurationKey] = ExtractParameterValue(response.Parameter.Value, parameterPropertyName);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Falha ao carregar o parametro '{parameterName}' para '{targetConfigurationKey}'.",
                ex);
        }
    }

    private static string? ResolveParameterName(
        IConfiguration configuration,
        IReadOnlyList<string> parameterNameConfigKeys,
        IReadOnlyList<string> parameterNameEnvironmentVariables)
    {
        foreach (var environmentVariable in parameterNameEnvironmentVariables)
        {
            var environmentValue = Environment.GetEnvironmentVariable(environmentVariable);
            if (!string.IsNullOrWhiteSpace(environmentValue))
            {
                return environmentValue;
            }
        }

        foreach (var configKey in parameterNameConfigKeys)
        {
            var configValue = configuration[configKey];
            if (!string.IsNullOrWhiteSpace(configValue))
            {
                return configValue;
            }
        }

        return null;
    }

    private static string ExtractParameterValue(string parameterValue, string parameterPropertyName)
    {
        try
        {
            using var document = JsonDocument.Parse(parameterValue);
            if (document.RootElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in document.RootElement.EnumerateObject())
                {
                    if (string.Equals(property.Name, parameterPropertyName, StringComparison.OrdinalIgnoreCase))
                    {
                        return property.Value.GetString() ?? string.Empty;
                    }
                }
            }
        }
        catch (JsonException)
        {
        }

        return parameterValue;
    }
}
