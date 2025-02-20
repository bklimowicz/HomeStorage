using HomeStorage.Domain.Exceptions;
using HomeStorage.Domain.ValueObjects;

namespace HomeStorage.Domain.Entities;

public class Product(ProductId id, 
    ProductName name, 
    Quantity quantity,
    Description? description = null,
    Producer? producer = null)
{
    public ProductId Id { get; private set; } = id;
    public ProductName Name { get; private set; } = name;
    public Quantity Quantity { get; private set; } = quantity;
    public Description? Description { get; set; } = description;
    public Producer? Producer { get; set; } = producer;

    public static Product Create(ProductId id,
        ProductName name, 
        Quantity quantity, 
        Description? description = null, 
        Producer? producer = null) => 
        new(id, name, quantity, description, producer);
    
    public void UpdateQuantity(Quantity quantity)
    {
        if (quantity < 0)
        {
            throw new InvalidQuantityException(quantity);
        }
        
        Quantity = quantity;
    }

    public void UpdateName(ProductName name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidNameException(name);
        }
        
        Name = name;
    }
}