namespace [assembly-generic].Common.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "[jwt-issuer-generic]";
    public string Audience { get; set; } = "[jwt-audience-generic]";
    public string ExpiryMinutes { get; set; } = "[jwt-expiry-minutes-generic]";
}
