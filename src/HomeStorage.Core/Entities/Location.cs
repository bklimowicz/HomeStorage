using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.Entities;

public class Location
{
    public LocationName LocationName { get; private set; }
    public IEnumerable<Product> Products => _products;
    private readonly List<Product> _products = [];
    
    public Location(LocationName locationName)
    {
        LocationName = locationName;
    }
    
    public void AddProduct(Product product)
    {
        _products.Add(product);
    }
    
    public void RemoveProduct(Product product)
    {
        _products.Remove(product);
    }
    
    public void MoveProductToNewLocation(Product product, Location newLocation)
    {
        _products.Remove(product);
        newLocation.AddProduct(product);
    }
}