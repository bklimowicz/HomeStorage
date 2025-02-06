using HomeStorage.Domain.Exceptions;

namespace HomeStorage.Domain.Model;

public class Product(string name, Location location, decimal quantity, string? producer, string? comments = null)
{
    public string Name { get; private set; } = string.IsNullOrWhiteSpace(name) ? throw new InvalidNameException(name) : name;
    public Location Location { get; private set; } = location ?? throw new InvalidLocationException();
    public decimal Quantity { get; private set; } = quantity < 0 ? throw new InvalidQuantityException(quantity) : quantity;
    public string? Comments { get; set; } = comments;
    public string? Producer { get; set; } = producer;

    public static Product Create(string name, Location location, decimal quantity, string? producer, string? comments = null) => 
        new(name, location, quantity, producer, comments);
    
    public void UpdateQuantity(decimal quantity)
    {
        if (quantity < 0)
        {
            throw new InvalidQuantityException(quantity);
        }
        
        Quantity = quantity;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidNameException(name);
        }
        
        Name = name;
    }

    public void UpdateLocation(Location location)
    {
        // any rules here?
        Location = location ?? throw new InvalidLocationException();
    }
}