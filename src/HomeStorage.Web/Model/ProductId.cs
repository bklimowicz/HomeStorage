using System.Text.Json.Serialization;

namespace HomeStorage.Web.Model;

public class ProductId
{
    [JsonPropertyName("value")]
    public Guid Value { get; set; }
}