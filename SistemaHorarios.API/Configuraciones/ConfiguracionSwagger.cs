using Microsoft.OpenApi.Models;

namespace SistemaHorarios.API.Configuraciones;

public static class ConfiguracionSwagger
{
    public static IServiceCollection ConfigureSwagger(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "SistemaHorarios.API",
                    Version = "v1"
                });

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",

                    Type = SecuritySchemeType.Http,

                    Scheme = "bearer",

                    BearerFormat = "JWT",

                    In = ParameterLocation.Header,

                    Description =
                        "Ingrese el token JWT así: Bearer {token}"
                });

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