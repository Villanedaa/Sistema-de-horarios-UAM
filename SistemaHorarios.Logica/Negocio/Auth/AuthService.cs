using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.DTOs.Auth;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Auth;

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
    public async Task<LoginResponseDto> Login(
        LoginRequestDto dto)
    {
        var usuario = await _context.Usuarios
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

            Rol = usuario.IdRol.ToString()
        };
    }
}