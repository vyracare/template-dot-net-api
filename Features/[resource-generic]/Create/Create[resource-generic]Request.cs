namespace [assembly-generic].Features.[resource-generic].Create;

public sealed record Create[resource-generic]Request(
    string Name,
    string? Code,
    string? Description,
    bool IsActive
);
