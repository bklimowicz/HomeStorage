namespace HomeStorage.Core.ValueObjects;

public sealed record Description
{
    public string Value { get; }
    
    public Description(string value)
    {
        Value = value;
    }
    
    public static implicit operator string(Description description) => description.Value;
    public static implicit operator Description(string value) => new(value);
}