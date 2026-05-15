using Microsoft.AspNetCore.Authorization;
using SistemaHorarios.Modelos.Constantes;

namespace SistemaHorarios.API.Politicas;

public static class PoliticasAutorizacion
{
    public static IServiceCollection
        ConfigureAuthorizationPolicies(
            this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Política exclusiva para administradores
            options.AddPolicy(
                "SoloAdministradores",
                policy =>
                    policy.RequireRole(
                        RolesSistema.Administrador
                    )
            );

            // Política para gestión de materias
            options.AddPolicy(
                "GestionMaterias",
                policy =>
                    policy.RequireRole(
                        RolesSistema.Administrador,
                        RolesSistema.Coordinador
                    )
            );

            // Política para visualización de horarios
            options.AddPolicy(
                "VerHorarios",
                policy =>
                    policy.RequireRole(
                        RolesSistema.Administrador,
                        RolesSistema.Coordinador,
                        RolesSistema.Docente
                    )
            );
        });

        return services;
    }
}