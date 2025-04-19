namespace HomeStorage.Domain.Exceptions;

internal class LocationNameEmptyException() : HomeStorageException("Invalid location")
{
}