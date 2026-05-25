namespace SistemaHorarios.API.Middleware;

/// <summary>
/// Middleware deshabilitado. Se conserva vacío para evitar referencias rotas
/// si quedó registrado en algún archivo local anterior.
/// </summary>
public class HistorialCambiosSeguroMiddleware
{
    private readonly RequestDelegate _next;

    public HistorialCambiosSeguroMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
    }
}
