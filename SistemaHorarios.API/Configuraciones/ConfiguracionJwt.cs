using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SistemaHorarios.API.Configuraciones;

/// <summary>
/// Clase encargada de configurar la autenticación JWT
/// del sistema.
///
/// Define:
/// - validación de tokens,
/// - emisor del token,
/// - audiencia válida,
/// - clave secreta,
/// - seguridad de autenticación.
///
/// Esta configuración permite proteger endpoints
/// utilizando [Authorize].
/// </summary>
public static class ConfiguracionJwt
{
    /// <summary>
    /// Configura el sistema de autenticación JWT
    /// para la aplicación.
    ///
    /// Registra:
    /// - autenticación Bearer,
    /// - validación de tokens,
    /// - validación de expiración,
    /// - validación de firma,
    /// - validación de issuer y audience.
    /// </summary>
    /// <param name="services">
    /// Colección de servicios de la aplicación.
    /// </param>
    /// <param name="configuration">
    /// Configuración general de la aplicación.
    /// </param>
    /// <returns>
    /// IServiceCollection configurado.
    /// </returns>
    public static IServiceCollection ConfigurarJWT(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Obtiene la configuración JWT
        // desde appsettings.json.
        var jwtSettings =
            configuration.GetSection("Jwt");

        services
            .AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // Configuración de validación
                // del token JWT.
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        // Valida que el emisor del token
                        // sea el correcto.
                        ValidateIssuer = true,

                        // Valida que el destinatario
                        // del token sea válido.
                        ValidateAudience = true,

                        // Verifica que el token
                        // no haya expirado.
                        ValidateLifetime = true,

                        // Valida la firma del token
                        // utilizando la clave secreta.
                        ValidateIssuerSigningKey = true,

                        // Emisor válido configurado.
                        ValidIssuer =
                            jwtSettings["Issuer"],

                        // Audiencia válida configurada.
                        ValidAudience =
                            jwtSettings["Audience"],

                        // Clave secreta utilizada para
                        // firmar y validar tokens JWT.
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    jwtSettings["Key"]!
                                )
                            )
                    };
            });

        return services;
    }
}