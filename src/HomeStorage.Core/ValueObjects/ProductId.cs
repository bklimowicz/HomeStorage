namespace HomeStorage.Core.ValueObjects;

public sealed record ProductId
{
    public Guid Value { get; }

    public ProductId(Guid value)
    {
        Value = value;
    }

    public static implicit operator Guid(ProductId productId) => productId.Value;
    public static implicit operator ProductId(Guid value) => new(value);
}