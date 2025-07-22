using HomeStorage.Core.DTOs;
using HomeStorage.Core.Exceptions;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.Entities;

public class Product
{
    private Product(ProductId id, 
        ProductName name, 
        Quantity quantity,
        Description? description = null,
        Producer? producer = null)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Description = description;
        Producer = producer;
    }

    public ProductId Id { get; private set; }
    public ProductName Name { get; private set; }
    public Quantity Quantity { get; private set; }
    public Description? Description { get; set; }
    public Producer? Producer { get; set; }

    public static Product Create(ProductId id,
        ProductName name,
        Quantity quantity,
        Description? description = null,
        Producer? producer = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidQuantityException(quantity);
        }

        if (quantity <= 0)
        {
            throw new InvalidNameException(name);
        }
        
        return new Product(id, name, quantity, description, producer);
    }
    
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

    public ProductDto ToDto()
    {
        return new ProductDto
        {
            Id = Id.Value,
            Name = Name.Value,
            Quantity = Quantity.Value,
            Description = Description?.Value ?? "",
            Producer = Producer?.Value ?? ""
        };
    }
}