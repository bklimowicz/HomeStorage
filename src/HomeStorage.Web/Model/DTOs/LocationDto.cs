using System.Text.Json.Serialization;

namespace HomeStorage.Web.Model.DTOs;

public class LocationDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("locationName")]
    public string LocationName { get; set; }
}
