using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Configuraciones;

public static class ConfiguracionComportamientoApi
{
    public static IServiceCollection
        ConfigurarComportamientoApi(
            this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(
            options =>
            {
                options
                    .InvalidModelStateResponseFactory =
                    context =>
                    {
                        var errores =
                            context.ModelState
                                .Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();

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