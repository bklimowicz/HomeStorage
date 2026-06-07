namespace HomeStorage.Core.Exceptions;

internal class LocationNameEmptyException() : HomeStorageException("Location name cannot be empty")
{
}