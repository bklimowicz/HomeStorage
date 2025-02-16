namespace HomeStorage.Domain.Model;

public class Location
{
    public string Name { get; }
    public IEnumerable<Product> Products => _products;
    private readonly IList<Product> _products;
    
    public Location(string name, IEnumerable<Product>? products)
    {
        Name = name;
        if (products is null)
        {
            _products = new List<Product>();
        }
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