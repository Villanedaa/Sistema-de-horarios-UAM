using System.Net;
using System.Text.Json;

namespace SistemaHorarios.API.Middleware;

/// <summary>
/// Middleware encargado de capturar excepciones globales
/// dentro de la aplicación.
///
/// Permite:
/// - interceptar errores no controlados,
/// - evitar caídas del servidor,
/// - devolver respuestas JSON uniformes,
/// - centralizar el manejo de excepciones.
///
/// Este middleware se ejecuta en cada request HTTP.
/// </summary>
public class ExceptionMiddleware
{
    /// <summary>
    /// Referencia al siguiente middleware
    /// dentro del pipeline HTTP.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// Constructor del middleware de excepciones.
    /// </summary>
    /// <param name="next">
    /// Siguiente middleware del pipeline.
    /// </param>
    public ExceptionMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Método principal del middleware.
    ///
    /// Ejecuta el siguiente middleware y captura
    /// cualquier excepción no controlada.
    /// </summary>
    /// <param name="context">
    /// Contexto HTTP actual.
    /// </param>
    public async Task InvokeAsync(
        HttpContext context)
    {
        try
        {
            // Continúa con el siguiente middleware.
            await _next(context);
        }
        catch (Exception ex)
        {
            // Captura errores globales
            // y genera respuesta controlada.
            await HandleExceptionAsync(
                context,
                ex
            );
        }
    }

    /// <summary>
    /// Construye la respuesta HTTP cuando ocurre
    /// una excepción no controlada.
    /// </summary>
    /// <param name="context">
    /// Contexto HTTP actual.
    /// </param>
    /// <param name="exception">
    /// Excepción capturada.
    /// </param>
    /// <returns>
    /// Respuesta JSON con información del error.
    /// </returns>
    private static Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        // Define el tipo de respuesta.
        context.Response.ContentType =
            "application/json";

        // Define código HTTP 500.
        context.Response.StatusCode =
            (int)HttpStatusCode.InternalServerError;

        // Estructura estándar de error.
        var response = new
        {
            success = false,

            message = exception.Message
        };

        // Convierte la respuesta a JSON.
        var json =
            JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}