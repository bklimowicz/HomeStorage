using HomeStorage.Domain.Exceptions;

namespace HomeStorage.Domain.Entities;

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