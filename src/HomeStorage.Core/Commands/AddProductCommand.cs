namespace HomeStorage.Core.Commands;

public record AddProductCommand(string Name, decimal Quantity, string Description, string Producer) : ICommand;