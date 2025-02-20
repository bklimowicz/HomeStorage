using HomeStorage.Domain.Entities;
using HomeStorage.Domain.Repositories;

namespace HomeStorage.Application.Commands.Handlers;

public class AddProductCommandHandler(IPostgresProductRepository repository) : ICommandHandler<AddProductCommand>
{
    public void Handle(AddProductCommand command)
    {
        var product = Product.Create(command.Id,
            command.Name,
            command.Quantity,
            command.Description!,
            command.Producer!);

        repository.Add(product);
    }
}