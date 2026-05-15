using Microsoft.EntityFrameworkCore;
using SistemaHorarios.API.Configuraciones;
using SistemaHorarios.API.Politicas;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Logica.Negocio.Materias;
using SistemaHorarios.Logica.Negocio.Roles.Interfaces;
using SistemaHorarios.Logica.Negocio.Roles.Servicios;

namespace SistemaHorarios.API.Extensions;

public static class ExtensionesServicios
{
    // Configura Entity Framework Core
    // y la conexión con MySQL.
    public static IServiceCollection
        ConfigurarBaseDatos(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        services.AddDbContext<SistemaHorariosDbContext>(
            options =>
                options.UseMySql(
                    configuration.GetConnectionString(
                        "DefaultConnection"
                    ),
                    ServerVersion.AutoDetect(
                        configuration.GetConnectionString(
                            "DefaultConnection"
                        )
                    )
                )
        );

        return services;
    }

    // Registra los servicios,
    // gestores y repositorios
    // utilizados por la aplicación.
    public static IServiceCollection
        RegistrarServicios(
            this IServiceCollection services)
    {
        // Servicios de autenticación
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<JwtService>();

        services.AddScoped<PasswordService>();


        // Repositorios
        services.AddScoped<MateriaRepository>();

        services.AddScoped<PrerrequisitoRepository>();


        // Gestores de lógica de negocio
        services.AddScoped<GestorMateria>();

        services.AddScoped<GestorPrerrequisito>();

        // Roles
        services.AddScoped<IRolService, RolService>();

        services.AddScoped<RolRepository>();

        return services;
    }

    // Configura políticas de autorización
    // basadas en roles y permisos.
    public static IServiceCollection
        ConfigurarPoliticas(
            this IServiceCollection services)
    {
        services.ConfigureAuthorizationPolicies();

        return services;
    }
}