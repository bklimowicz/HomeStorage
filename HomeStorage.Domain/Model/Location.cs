namespace HomeStorage.Domain.Model;

public class Location
{
    public string Name { get; }
    
    public Location(string name)
    {
        Name = name;
    }
}