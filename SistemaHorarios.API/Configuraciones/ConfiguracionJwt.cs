using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SistemaHorarios.API.Configuraciones;

public static class ConfiguracionJwt
{
    public static IServiceCollection ConfigurarJWT(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings =
            configuration.GetSection("Jwt");

        var key =
            jwtSettings["Key"];

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new Exception(
                "La clave JWT no está configurada en appsettings.json.");
        }

        services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(key)
                            )
                    };
            });

        return services;
    }
}