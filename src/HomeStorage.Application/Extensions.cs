using HomeStorage.Application.Commands;
using HomeStorage.Application.Commands.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace HomeStorage.Application;

public static class Extensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<AddProductCommand>, AddProductCommandHandler>();
    }
}