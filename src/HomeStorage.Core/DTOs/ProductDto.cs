namespace HomeStorage.Core.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Quantity { get; set; }
    public string? Description { get; set; }
    public string? Producer { get; set; }
}