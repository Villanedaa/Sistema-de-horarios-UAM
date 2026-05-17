using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Configuraciones;

/// <summary>
/// Clase encargada de configurar el comportamiento
/// global de la API.
///
/// Permite personalizar:
/// - validaciones automáticas,
/// - respuestas HTTP,
/// - manejo de ModelState,
/// - estructura de errores.
///
/// Centraliza configuraciones relacionadas con
/// el comportamiento general de ASP.NET Core API.
/// </summary>
public static class ConfiguracionComportamientoApi
{
    /// <summary>
    /// Configura el comportamiento automático
    /// de validación de modelos de ASP.NET Core.
    ///
    /// Personaliza la respuesta enviada cuando
    /// el ModelState es inválido.
    ///
    /// Esto permite:
    /// - respuestas uniformes,
    /// - mejor manejo frontend,
    /// - estructura consistente de errores.
    /// </summary>
    /// <param name="services">
    /// Colección de servicios de la aplicación.
    /// </param>
    /// <returns>
    /// IServiceCollection configurado.
    /// </returns>
    public static IServiceCollection
        ConfigurarComportamientoApi(
            this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(
            options =>
            {
                // Personaliza la respuesta automática
                // cuando las validaciones fallan.
                options
                    .InvalidModelStateResponseFactory =
                    context =>
                    {
                        // Obtiene todos los mensajes
                        // de error del ModelState.
                        var errores =
                            context.ModelState
                                .Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();

                        // Devuelve respuesta HTTP 400
                        // con estructura estandarizada.
                        return new BadRequestObjectResult(
                            new
                            {
                                success = false,

                                message =
                                    "Datos inválidos",

                                errors = errores
                            });
                    };
            });

        return services;
    }
}