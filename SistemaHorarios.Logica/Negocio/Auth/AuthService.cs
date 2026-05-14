using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.DTOs.Auth;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Auth;

/// <summary>
/// Servicio encargado de gestionar
/// autenticación y autorización
/// de usuarios.
/// </summary>
public class AuthService : IAuthService
{
    private readonly SistemaHorariosDbContext _context;

    private readonly JwtService _jwtService;

    private readonly PasswordService _passwordService;

    /// <summary>
    /// Inicializa una nueva instancia
    /// del servicio de autenticación.
    /// </summary>
    /// <param name="context">
    /// Contexto Entity Framework Core.
    /// </param>
    /// <param name="jwtService">
    /// Servicio encargado de generar JWT.
    /// </param>
    /// <param name="passwordService">
    /// Servicio encargado de hash y validación
    /// de contraseñas.
    /// </param>
    public AuthService(
        SistemaHorariosDbContext context,
        JwtService jwtService,
        PasswordService passwordService)
    {
        _context = context;

        _jwtService = jwtService;

        _passwordService = passwordService;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="dto">
    /// Información del usuario a registrar.
    /// </param>
    public async Task Registrar(
        RegistroUsuarioDto dto)
    {
        var existeUsuario =
            await _context.Usuarios
                .AnyAsync(u =>
                    u.CorreoInstitucional ==
                    dto.CorreoInstitucional);

        if (existeUsuario)
        {
            throw new Exception(
                "El usuario ya existe");
        }

        var passwordHash =
            _passwordService.HashPassword(
                dto.Contrasena
            );

        var usuario = new Usuario
        {
            NombreCompleto =
                dto.NombreCompleto,

            Cedula =
                dto.Cedula,

            CorreoInstitucional =
                dto.CorreoInstitucional,

            ContrasenaHash =
                passwordHash,

            IdRol =
                dto.IdRol,

            Estado = "Activo"
        };

        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Valida credenciales de usuario y genera JWT.
    /// </summary>
    /// <param name="dto">
    /// Credenciales usuario.
    /// </param>
    /// <returns>
    /// Información autenticación y token JWT.
    /// </returns>
    public async Task<LoginResponseDto> Login(
        LoginRequestDto dto)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u =>
                u.CorreoInstitucional ==
                dto.CorreoInstitucional);

        if (usuario == null)
        {
            throw new Exception(
                "Usuario no encontrado");
        }

        var passwordValida =
            _passwordService.VerifyPassword(
                dto.Contrasena,
                usuario.ContrasenaHash);

        if (!passwordValida)
        {
            throw new Exception(
                "Contraseña incorrecta");
        }

        var token =
            _jwtService.GenerarToken(usuario);

        return new LoginResponseDto
        {
            Token = token,

            NombreCompleto =
                usuario.NombreCompleto,

            Rol = usuario.Rol.Nombre
        };
    }

    /// <summary>
    /// Obtiene información del usuario autenticado.
    /// </summary>
    /// <param name="idUsuario">
    /// Identificador usuario.
    /// </param>
    /// <returns>
    /// Información perfil usuario.
    /// </returns>
    public async Task<PerfilUsuarioDto>
    ObtenerPerfil(int idUsuario)
    {
        var usuario =
            await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(
                    u => u.IdUsuario == idUsuario);

        if (usuario == null)
        {
            throw new Exception(
                "Usuario no encontrado");
        }

        return new PerfilUsuarioDto
        {
            IdUsuario = usuario.IdUsuario,

            NombreCompleto =
                usuario.NombreCompleto,

            CorreoInstitucional =
                usuario.CorreoInstitucional,

            Rol =
                usuario.Rol.Nombre
        };
    }
}