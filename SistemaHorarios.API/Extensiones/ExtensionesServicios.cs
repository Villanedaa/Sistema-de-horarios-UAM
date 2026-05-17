using Microsoft.EntityFrameworkCore;
using SistemaHorarios.API.Politicas;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Logica.Negocio.FranjasHorarias.Interfaces;
using SistemaHorarios.Logica.Negocio.FranjasHorarias.Servicios;
using SistemaHorarios.Logica.Negocio.Materias;
using SistemaHorarios.Logica.Negocio.Roles.Interfaces;
using SistemaHorarios.Logica.Negocio.Roles.Servicios;
using SistemaHorarios.Logica.Negocio.Usuario.Interface;
using SistemaHorarios.Logica.Negocio.Usuario.Servicios;
using SistemaHorarios.Logica.Negocio.PlanAcademico.Interfaces;
using SistemaHorarios.Logica.Negocio.PlanAcademico.Servicios;
using SistemaHorarios.Logica.Negocio.Reportes.Interfaces;
using SistemaHorarios.Logica.Negocio.Reportes.Servicios;

namespace SistemaHorarios.API.Extensions;

public static class ExtensionesServicios
{
    public static IServiceCollection ConfigurarBaseDatos(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<SistemaHorariosDbContext>(
            options =>
                options.UseMySql(
                    connectionString,
                    ServerVersion.AutoDetect(connectionString)
                )
        );

        return services;
    }

    public static IServiceCollection RegistrarServicios(
        this IServiceCollection services)
    {
        // Autenticación
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<JwtService>();
        services.AddScoped<PasswordService>();

        // Usuarios
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        // Roles
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<RolRepository>();

        // Materias
        services.AddScoped<GestorMateria>();
        services.AddScoped<GestorPrerrequisito>();
        services.AddScoped<MateriaRepository>();
        services.AddScoped<PrerrequisitoRepository>();

        // Franjas horarias
        services.AddScoped<IFranjaHorariaService, FranjaHorariaService>();
        services.AddScoped<FranjaHorariaRepository>();

        // Plan académico
        services.AddScoped<IPlanAcademicoService, PlanAcademicoService>();
        services.AddScoped<IPlanAcademicoRepository, PlanAcademicoRepository>();

        // Reportes
        services.AddScoped<IReporteService, ReporteService>();

        return services;
    }

    public static IServiceCollection ConfigurarPoliticas(
        this IServiceCollection services)
    {
        services.ConfigureAuthorizationPolicies();

        return services;
    }
}