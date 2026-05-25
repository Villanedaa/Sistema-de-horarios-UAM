namespace SistemaHorarios.API.Middleware;

/// <summary>
/// Middleware deshabilitado. No registra auditoría ni afecta el flujo normal.
/// </summary>
public class AuditoriaSesionMiddleware
{
    private readonly RequestDelegate _next;

    public AuditoriaSesionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
    }
}
