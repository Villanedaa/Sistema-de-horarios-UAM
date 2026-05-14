using Microsoft.AspNetCore.Authorization;

namespace SistemaHorarios.API.Politicas;

public static class PoliticasAutorizacion
{
    public static IServiceCollection
        ConfigureAuthorizationPolicies(
            this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "SoloAdministradores",
                policy =>
                    policy.RequireRole(
                        "Administrador"));

            options.AddPolicy(
                "GestionMaterias",
                policy =>
                    policy.RequireRole(
                        "Administrador",
                        "Coordinador"));

            options.AddPolicy(
                "VerHorarios",
                policy =>
                    policy.RequireRole(
                        "Administrador",
                        "Coordinador",
                        "Docente"));
        });

        return services;
    }
}