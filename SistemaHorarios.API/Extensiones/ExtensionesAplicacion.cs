using SistemaHorarios.API.Middleware;

namespace SistemaHorarios.API.Extensions;

public static class ExtensionesAplicacion
{
    public static WebApplication ConfigurarPipeline(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI();
        }

        // En desarrollo local se comenta para evitar errores
        // de Swagger con http/https.
        // app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}