using HomeStorage.Domain.Exceptions;

namespace HomeStorage.Domain.ValueObjects;

public sealed record Quantity {
    public decimal Value { get; }

    public Quantity(decimal value)
    {
        if (value < 0)
        {
            throw new InvalidQuantityException(value);
        }
        
        Value = value;
    }

    public static implicit operator decimal(Quantity quantity) => quantity.Value;
    public static implicit operator Quantity(decimal value) => new(value);
}