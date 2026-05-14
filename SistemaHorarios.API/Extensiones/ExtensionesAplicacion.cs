using SistemaHorarios.API.Middleware;

namespace SistemaHorarios.API.Extensions;

public static class ExtensionesAplicacion
{
    // Configura el pipeline HTTP principal
    // de la aplicación.
    public static WebApplication
        ConfigurarPipeline(
            this WebApplication app)
    {
        // Habilita Swagger únicamente
        // en entorno de desarrollo.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI();
        }

        // Redirecciona automáticamente
        // las peticiones HTTP a HTTPS.
        app.UseHttpsRedirection();

        // Middleware global para manejo
        // de excepciones.
        app.UseMiddleware<ExceptionMiddleware>();

        // Habilita autenticación JWT.
        app.UseAuthentication();

        // Habilita autorización
        // basada en roles y policies.
        app.UseAuthorization();

        // Mapea los endpoints
        // de los controladores.
        app.MapControllers();

        return app;
    }
}