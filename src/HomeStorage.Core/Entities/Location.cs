using HomeStorage.Core.ValueObjects;

namespace HomeStorage.Core.Entities;

public class Location
{
    public int Id { get; private set; }
    public LocationName LocationName { get; private set; }
    public IEnumerable<Product> Products => _products;
    private readonly List<Product> _products = [];
    
    public Location(int id, LocationName locationName)
    {
        LocationName = locationName;
        Id = id;
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