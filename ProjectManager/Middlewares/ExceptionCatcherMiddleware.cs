using System.Net;
using BusinessLogic;
using FluentValidation;

namespace ProjectManager.Middlewares;

public class ExceptionCatcherMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionCatcherMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionCatcherMiddleware> logger /* other dependencies */)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorDto(string.Join("\n", ex.Errors.Select(x => x.ErrorMessage))));
        }
        catch (BusinessException ex)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorDto(ex.Message));
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Unhandled Exception");
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new ErrorDto("Непредвиденная ошибка"));
        }
    }
}