using System.Net;
using System.Text.Json;

namespace SistemaHorarios.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ManejarErrorAsync(context, ex);
        }
    }

    private static async Task ManejarErrorAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode =
            (int)HttpStatusCode.InternalServerError;

        var respuesta = new
        {
            success = false,
            message = exception.Message
        };

        var json =
            JsonSerializer.Serialize(respuesta);

        await context.Response.WriteAsync(json);
    }
}