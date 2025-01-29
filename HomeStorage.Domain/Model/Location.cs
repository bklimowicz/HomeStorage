namespace HomeStorage.Domain.Model;

public class Location
{
    public string Name { get; }
    public IEnumerable<Product> Products => _products;
    private IList<Product> _products;
    
    public Location(string name, IEnumerable<Product>? products)
    {
        Name = name;
        if (products is null)
        {
            _products = new List<Product>();
        }
    }
}