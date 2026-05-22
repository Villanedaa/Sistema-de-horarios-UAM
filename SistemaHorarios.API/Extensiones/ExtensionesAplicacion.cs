using SistemaHorarios.API.Middleware;

namespace SistemaHorarios.API.Extensions;

public static class ExtensionesAplicacion
{
    public static WebApplication ConfigurarPipeline(
        this WebApplication app)
    {
        var swaggerHabilitado =
            app.Environment.IsDevelopment() ||
            app.Configuration.GetValue<bool>("Swagger:Enabled");

        if (swaggerHabilitado)
        {
            app.UseSwagger();

            app.UseSwaggerUI();
        }

        // En desarrollo local se comenta para evitar errores
        // de Swagger con http/https.
        // app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionMiddleware>();

        app.UseCors("PermitirFrontend");

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}