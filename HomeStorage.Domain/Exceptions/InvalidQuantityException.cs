namespace HomeStorage.Domain.Exceptions;

internal class InvalidQuantityException(decimal quantity) : HomeStorageException("Invalid quantity")
{
    public decimal Quantity { get; } = quantity;
}