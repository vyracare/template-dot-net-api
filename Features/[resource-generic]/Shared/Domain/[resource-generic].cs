namespace [assembly-generic].Features.[resource-generic].Shared.Domain;

public sealed class [resource-generic]
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
