using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace [assembly-generic].Infrastructure.Persistence.Documents;

/// <summary>
/// Representa o formato persistido no MongoDB para esta entidade.
/// </summary>
public sealed class [resource-generic]Document
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
/// <summary>
/// Identificador do registro ou do recurso processado.
/// </summary>
    public string? Id { get; set; }

/// <summary>
/// Nome principal associado ao recurso.
/// </summary>
    public string Name { get; set; } = string.Empty;
/// <summary>
/// C?digo interno usado para identificar o recurso no dom?nio.
/// </summary>
    public string? Code { get; set; }
/// <summary>
/// Descri??o textual usada para complementar o entendimento do recurso.
/// </summary>
    public string? Description { get; set; }
/// <summary>
/// Indica se o recurso est? habilitado para uso.
/// </summary>
    public bool IsActive { get; set; }
/// <summary>
/// Data de cria??o do registro.
/// </summary>
    public DateTime CreatedAt { get; set; }
/// <summary>
/// Data da ?ltima atualiza??o do registro.
/// </summary>
    public DateTime UpdatedAt { get; set; }
}
