using System.Text.Json.Serialization;

namespace HomeStorage.Web.Model;

public class Product
{
    [JsonPropertyName("id")]
    public ProductId Id { get; set; }
    [JsonPropertyName("name")]
    public ProductName Name { get; set; }
    [JsonPropertyName("description")]
    public ProductDescription Description { get; set; }
}