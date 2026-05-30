namespace [assembly-generic].Common.Configuration;

/// <summary>
/// Representa uma configura??o tipada lida do appsettings ou das vari?veis de ambiente.
/// </summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

/// <summary>
/// Obt?m ou define k ey.
/// </summary>
    public string Key { get; set; } = string.Empty;
/// <summary>
/// Obt?m ou define i ss ue r.
/// </summary>
    public string Issuer { get; set; } = "[jwt-issuer-generic]";
/// <summary>
/// Obt?m ou define a ud ie nc e.
/// </summary>
    public string Audience { get; set; } = "[jwt-audience-generic]";
/// <summary>
/// Obt?m ou define e xp ir ym in ut es.
/// </summary>
    public string ExpiryMinutes { get; set; } = "[jwt-expiry-minutes-generic]";
}
