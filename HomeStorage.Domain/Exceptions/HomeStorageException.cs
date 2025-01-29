namespace HomeStorage.Domain.Exceptions;

internal abstract class HomeStorageException(string errorMessage) : Exception
{
    public string ErrorMessage { get; } = errorMessage;
}