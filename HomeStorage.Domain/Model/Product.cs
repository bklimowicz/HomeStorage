using HomeStorage.Domain.Exceptions;

namespace HomeStorage.Domain.Model;

public class Product
{
    public string Name { get; private set; }
    public Location Location { get; private set; }
    public decimal Quantity { get; private set; }
    public string Comments { get; set; }
    public string Producer { get; set; }
    
    public Product(string name, Location location, decimal quantity, string comments)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new InvalidNameException(name) : name;
        Location = location ?? throw new InvalidLocationException();
        Quantity = quantity < 0 ? throw new InvalidQuantityException(quantity) : quantity;
        Comments = comments;
    }

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