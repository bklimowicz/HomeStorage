using System.Text.Json.Serialization;

namespace HomeStorage.Web.Model;

public class ProductDescription
{
    [JsonPropertyName("value")]
    public string Value { get; set; }
}