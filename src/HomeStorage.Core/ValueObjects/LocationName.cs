using HomeStorage.Core.Exceptions;

namespace HomeStorage.Core.ValueObjects;

public sealed record LocationName
{
    public string Value { get; }
    
    public LocationName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new LocationNameEmptyException();
        }
        Value = value;
    }
    
    public static implicit operator string(LocationName location) => location.Value;
    public static implicit operator LocationName(string value) => new(value);
}