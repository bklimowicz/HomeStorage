using System.Text.Json.Serialization;

namespace HomeStorage.Web.Model;

public class ProductName
{
    [JsonPropertyName("value")]
    public string Value { get; set; } 
    
}