using Microsoft.OpenApi.Models;

namespace SistemaHorarios.API.Configuraciones;

/// <summary>
/// Clase encargada de configurar Swagger/OpenAPI
/// para la documentación interactiva de la API.
///
/// Permite:
/// - visualizar endpoints,
/// - probar requests,
/// - documentar la API,
/// - autenticar mediante JWT desde Swagger.
///
/// Facilita las pruebas y el desarrollo
/// del backend.
/// </summary>
public static class ConfiguracionSwagger
{
    /// <summary>
    /// Configura Swagger para la aplicación.
    ///
    /// Registra:
    /// - documentación OpenAPI,
    /// - versión de la API,
    /// - autenticación JWT Bearer,
    /// - autorización de endpoints protegidos.
    /// </summary>
    /// <param name="services">
    /// Colección de servicios de la aplicación.
    /// </param>
    /// <returns>
    /// IServiceCollection configurado.
    /// </returns>
    public static IServiceCollection ConfigurarSwagger(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Configuración principal
            // de la documentación Swagger.
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "SistemaHorarios.API",

                    Version = "v1"
                });

            // Configura autenticación JWT
            // dentro de Swagger.
            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    // Nombre del header HTTP
                    // utilizado para autenticación.
                    Name = "Authorization",

                    // Tipo de autenticación HTTP.
                    Type = SecuritySchemeType.Http,

                    // Esquema Bearer.
                    Scheme = "bearer",

                    // Formato del token.
                    BearerFormat = "JWT",

                    // El token será enviado
                    // en el header.
                    In = ParameterLocation.Header,

                    // Descripción mostrada
                    // en Swagger UI.
                    Description =
                        "Ingrese el token JWT así: Bearer {token}"
                });

            // Aplica la seguridad JWT
            // a los endpoints protegidos.
            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference =
                                new OpenApiReference
                                {
                                    Type =
                                        ReferenceType
                                            .SecurityScheme,

                                    Id = "Bearer"
                                }
                        },

                        Array.Empty<string>()
                    }
                });
        });

        return services;
    }
}