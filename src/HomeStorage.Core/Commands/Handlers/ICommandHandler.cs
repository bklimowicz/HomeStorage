namespace HomeStorage.Core.Commands.Handlers;

public interface ICommandHandler
{
    Task HandleAsync(ICommand command);
}