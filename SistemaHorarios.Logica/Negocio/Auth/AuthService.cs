using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.DTOs.Auth;
using UsuarioEntidad = SistemaHorarios.Modelos.Entidades.Usuario;

namespace SistemaHorarios.Logica.Negocio.Auth;

/// <summary>
/// Servicio encargado de gestionar autenticación y autorización de usuarios.
/// </summary>
public class AuthService : IAuthService
{
    private readonly SistemaHorariosDbContext _context;
    private readonly JwtService _jwtService;
    private readonly PasswordService _passwordService;

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
    public async Task Registrar(RegistroUsuarioDto dto)
    {
        bool existeCorreo =
            await _context.Usuarios.AnyAsync(u =>
                u.CorreoInstitucional == dto.CorreoInstitucional);

        if (existeCorreo)
        {
            throw new Exception("El correo ya está registrado.");
        }

        bool existeCedula =
            await _context.Usuarios.AnyAsync(u =>
                u.Cedula == dto.Cedula);

        if (existeCedula)
        {
            throw new Exception("La cédula ya está registrada.");
        }

        string passwordHash =
            _passwordService.HashPassword(dto.Contrasena);

        var usuario = new UsuarioEntidad
        {
            NombreCompleto = dto.NombreCompleto,
            Cedula = dto.Cedula,
            CorreoInstitucional = dto.CorreoInstitucional,
            ContrasenaHash = passwordHash,
            IdRol = dto.IdRol,
            Estado = "Activo"
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Valida credenciales y genera JWT si el usuario está activo.
    /// </summary>
    public async Task<LoginResponseDto> Login(LoginRequestDto dto)
    {
        var usuario =
            await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u =>
                    u.CorreoInstitucional == dto.CorreoInstitucional);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado.");
        }

        bool passwordValida =
            _passwordService.VerifyPassword(
                dto.Contrasena,
                usuario.ContrasenaHash);

        if (!passwordValida)
        {
            throw new Exception("Contraseña incorrecta.");
        }

        if (!UsuarioEstaActivo(usuario.Estado))
        {
            throw new Exception("El usuario se encuentra inactivo. No puede iniciar sesión.");
        }

        string token = _jwtService.GenerarToken(usuario);

        return new LoginResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            CorreoInstitucional = usuario.CorreoInstitucional,
            Token = token,
            NombreCompleto = usuario.NombreCompleto,
            Rol = usuario.Rol?.Nombre ?? string.Empty
        };
    }

    /// <summary>
    /// Obtiene información del usuario autenticado.
    /// </summary>
    public async Task<PerfilUsuarioDto> ObtenerPerfil(int idUsuario)
    {
        var usuario =
            await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u =>
                    u.IdUsuario == idUsuario);

        if (usuario == null)
        {
            throw new Exception("Usuario no encontrado.");
        }

        return new PerfilUsuarioDto
        {
            IdUsuario = usuario.IdUsuario,
            NombreCompleto = usuario.NombreCompleto,
            CorreoInstitucional = usuario.CorreoInstitucional,
            Rol = usuario.Rol?.Nombre ?? string.Empty
        };
    }

    /// <summary>
    /// Determina si el estado permite acceso al sistema.
    /// </summary>
    private static bool UsuarioEstaActivo(string? estado)
    {
        return string.Equals(
            estado?.Trim(),
            "Activo",
            StringComparison.OrdinalIgnoreCase);
    }
}
