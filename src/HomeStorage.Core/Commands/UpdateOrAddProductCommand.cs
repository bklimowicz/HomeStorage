namespace HomeStorage.Core.Commands;

public record UpdateOrAddProductCommand(string Name, decimal Quantity, string Description, string Producer) : ICommand;