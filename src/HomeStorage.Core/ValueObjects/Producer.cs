namespace HomeStorage.Core.ValueObjects;

public sealed record Producer
{
    public string Value { get; }
    
    public Producer(string value)
    {
        Value = value;
    }
    
    public static implicit operator string(Producer producer) => producer.Value;
    public static implicit operator Producer(string value) => new(value);
}