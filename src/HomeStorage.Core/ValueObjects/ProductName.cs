using HomeStorage.Core.Exceptions;

namespace HomeStorage.Core.ValueObjects;

public sealed record ProductName
{
    public string Value { get; }

    public ProductName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidNameException(value);
        }
        
        Value = value;
    }

    public static implicit operator string(ProductName productName) => productName.Value;
    public static implicit operator ProductName(string value) => new(value);
}