namespace HomeStorage.Core.Exceptions;

internal class InvalidNameException(string name) : HomeStorageException("Name cannot be empty")
{
    public string Name { get; } = name;
}