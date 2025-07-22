using System.Text.Json.Serialization;

namespace HomeStorage.Web.Model.DTOs;

public class ProductDto
{
   [JsonPropertyName("id")]
   public Guid? Id { get; set; }
   [JsonPropertyName("name")]
   public string Name { get; set; }
   [JsonPropertyName("description")]
   public string? Description { get; set; }
   [JsonPropertyName("producer")]
   public string? Producer { get; set; }
   [JsonPropertyName("quantity")]
   public decimal Quantity { get; set; }
}