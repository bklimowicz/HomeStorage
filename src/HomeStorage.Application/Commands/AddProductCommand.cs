namespace HomeStorage.Application.Commands;

public record AddProductCommand(Guid Id, 
    string Name, 
    int Quantity, 
    string? Description = null, 
    string? Producer = null) : ICommand;