using System.Net;
using HomeStorage.Core.Exceptions;

namespace HomeStorage.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        ErrorResponse response;

        switch (exception)
        {
            case InvalidNameException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new ErrorResponse(ex.ErrorMessage, StatusCodes.Status400BadRequest);
                break;

            case InvalidQuantityException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new ErrorResponse(ex.ErrorMessage, StatusCodes.Status400BadRequest);
                break;

            case LocationNameEmptyException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new ErrorResponse(ex.ErrorMessage, StatusCodes.Status400BadRequest);
                break;

            case HomeStorageException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new ErrorResponse(ex.ErrorMessage, StatusCodes.Status400BadRequest);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response = new ErrorResponse("An internal server error occurred", StatusCodes.Status500InternalServerError);
                break;
        }

        return context.Response.WriteAsJsonAsync(response);
    }
}

public sealed record ErrorResponse(string Message, int StatusCode)
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
