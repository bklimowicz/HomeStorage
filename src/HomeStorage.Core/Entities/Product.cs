using HomeStorage.Core.DTOs;
using HomeStorage.Core.Exceptions;
using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.Entities;

public class Product
{
    private Product(ProductId id,
        ProductName name,
        Quantity quantity,
        int locationId,
        Description? description = null,
        Producer? producer = null)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        LocationId = locationId;
        Description = description;
        Producer = producer;
    }

    public ProductId Id { get; private set; }
    public ProductName Name { get; private set; }
    public Quantity Quantity { get; private set; }
    public int LocationId { get; private set; }
    public Location? Location { get; private set; }
    public Description? Description { get; set; }
    public Producer? Producer { get; set; }

    public static Product Create(ProductId id,
        ProductName name,
        Quantity quantity,
        int locationId,
        Description? description = null,
        Producer? producer = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidNameException(name);
        }

        if (quantity <= 0)
        {
            throw new InvalidQuantityException(quantity);
        }

        return new Product(id, name, quantity, locationId, description, producer);
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

    public void MoveToLocation(int locationId) => LocationId = locationId;

    public ProductDto ToDto()
    {
        return new ProductDto
        {
            Id = Id.Value,
            Name = Name.Value,
            Quantity = Quantity.Value,
            LocationId = LocationId,
            LocationName = Location?.LocationName.Value,
            Description = Description?.Value ?? "",
            Producer = Producer?.Value ?? ""
        };
    }
}
