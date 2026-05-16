using SistemaHorarios.API.Middleware;

namespace SistemaHorarios.API.Extensiones;

/// <summary>
/// Contiene métodos de extensión para configurar
/// el pipeline HTTP de la aplicación.
/// </summary>
public static class ExtensionesAplicacion
{
    /// <summary>
    /// Configura los middlewares principales de la aplicación.
    /// </summary>
    /// <param name="app">
    /// Instancia principal de la aplicación web.
    /// </param>
    /// <returns>
    /// Aplicación web configurada.
    /// </returns>
    public static WebApplication ConfigurarPipeline(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}