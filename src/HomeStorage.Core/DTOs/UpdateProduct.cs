namespace HomeStorage.Core.DTOs;

public record UpdateProduct
{
    public string Name { get; set; }
    public decimal Quantity { get; set; }
    public string? Description { get; set; }
    public string? Producer { get; set; }
}