namespace [assembly-generic].Features.[resource-generic].Create;

/// <summary>
/// Define o contrato de entrada esperado por este caso de uso.
/// </summary>
public sealed record Create[resource-generic]Request(
    string Name,
    string? Code,
    string? Description,
    bool IsActive
);
