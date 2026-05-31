namespace [assembly-generic].Common.Configuration;

/// <summary>
/// Representa as opções de configuração carregadas da aplicação.
/// </summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

/// <summary>
/// Obtém ou define a chave usada no processo de autenticação ou assinatura.
/// </summary>
    public string Key { get; set; } = string.Empty;
/// <summary>
/// Obtém ou define o emissor considerado válido para o token.
/// </summary>
    public string Issuer { get; set; } = "[jwt-issuer-generic]";
/// <summary>
/// Obtém ou define o público considerado válido para o token.
/// </summary>
    public string Audience { get; set; } = "[jwt-audience-generic]";
/// <summary>
/// Obtém ou define a quantidade de minutos de validade do token.
/// </summary>
    public string ExpiryMinutes { get; set; } = "[jwt-expiry-minutes-generic]";
}
