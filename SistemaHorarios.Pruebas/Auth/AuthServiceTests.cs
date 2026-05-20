using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Modelos.DTOs.Auth;
using SistemaHorarios.Modelos.Entidades;
using SistemaHorarios.Pruebas.Helpers;
using Xunit;

namespace SistemaHorarios.Pruebas.Auth;

public class AuthServiceTests : IDisposable
{
    private readonly SistemaHorariosDbContext _context;
    private readonly PasswordService _passwordService;
    private readonly AuthService _sut;

    public AuthServiceTests()
    {
        _context = TestDbContextFactory.CrearContexto();
        _passwordService = new PasswordService();
        _sut = new AuthService(
            _context,
            TestConfigurationFactory.CrearJwtService(),
            _passwordService);
    }

    [Fact]
    public async Task Login_ConCredencialesValidas_RetornaTokenYDatosUsuario()
    {
        // Arrange
        await SembrarUsuarioAsync("juan@uam.edu", "123456", "Juan Pérez", "Admin");

        // Act
        var resultado = await _sut.Login(new LoginRequestDto
        {
            CorreoInstitucional = "juan@uam.edu",
            Contrasena = "123456"
        });

        // Assert
        resultado.Token.Should().NotBeNullOrWhiteSpace();
        resultado.NombreCompleto.Should().Be("Juan Pérez");
        resultado.Rol.Should().Be("Admin");
    }

    [Fact]
    public async Task Login_ConCorreoInexistente_LanzaExcepcion()
    {
        // Arrange
        var solicitud = new LoginRequestDto
        {
            CorreoInstitucional = "noexiste@autonoma.edu.co",
            Contrasena = "123456"
        };

        // Act
        var accion = async () => await _sut.Login(solicitud);

        // Assert
        await accion.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Usuario no encontrado.");
    }

    [Fact]
    public async Task Login_ConContrasenaIncorrecta_LanzaExcepcion()
    {
        // Arrange
        await SembrarUsuarioAsync("juan@uam.edu", "123456", "Juan Pérez", "Admin");

        // Act
        var accion = async () => await _sut.Login(new LoginRequestDto
        {
            CorreoInstitucional = "juan@uam.edu",
            Contrasena = "incorrecta"
        });

        // Assert
        await accion.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Contraseña incorrecta.");
    }

    [Fact]
    public async Task Registrar_ConCorreoDuplicado_LanzaExcepcion()
    {
        // Arrange
        await SembrarUsuarioAsync("juan@uam.edu", "123456", "Juan Pérez", "Admin");

        var dto = new RegistroUsuarioDto
        {
            NombreCompleto = "Otro Usuario",
            Cedula = "002",
            CorreoInstitucional = "juan@uam.edu",
            Contrasena = "123456",
            IdRol = 1
        };

        // Act
        var accion = async () => await _sut.Registrar(dto);

        // Assert
        await accion.Should()
            .ThrowAsync<Exception>()
            .WithMessage("El correo ya está registrado.");
    }

    [Fact]
    public async Task ObtenerPerfil_ConUsuarioExistente_RetornaDatosPerfil()
    {
        // Arrange
        await SembrarUsuarioAsync("juan@uam.edu", "123456", "Juan Pérez", "Admin");

        // Act
        var perfil = await _sut.ObtenerPerfil(1);

        // Assert
        perfil.IdUsuario.Should().Be(1);
        perfil.NombreCompleto.Should().Be("Juan Pérez");
        perfil.CorreoInstitucional.Should().Be("juan@uam.edu");
        perfil.Rol.Should().Be("Admin");
    }

    [Fact]
    public async Task Registrar_ConDatosValidos_PersisteUsuario()
    {
        // Arrange
        await SembrarRolAsync();

        var dto = new RegistroUsuarioDto
        {
            NombreCompleto = "María López",
            Cedula = "002",
            CorreoInstitucional = "maria@uam.edu",
            Contrasena = "123456",
            IdRol = 1
        };

        // Act
        await _sut.Registrar(dto);

        // Assert
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.CorreoInstitucional == "maria@uam.edu");

        usuario.Should().NotBeNull();
        usuario!.NombreCompleto.Should().Be("María López");
        usuario.Estado.Should().Be("Activo");
        _passwordService.VerifyPassword("123456", usuario.ContrasenaHash)
            .Should().BeTrue();
    }

    [Fact]
    public async Task Registrar_ConCedulaDuplicada_LanzaExcepcion()
    {
        // Arrange
        await SembrarUsuarioAsync("juan@uam.edu", "123456", "Juan Pérez", "Admin");

        var dto = new RegistroUsuarioDto
        {
            NombreCompleto = "Otro Usuario",
            Cedula = "001",
            CorreoInstitucional = "otro@uam.edu",
            Contrasena = "123456",
            IdRol = 1
        };

        // Act
        var accion = async () => await _sut.Registrar(dto);

        // Assert
        await accion.Should()
            .ThrowAsync<Exception>()
            .WithMessage("La cédula ya está registrada.");
    }

    [Fact]
    public async Task ObtenerPerfil_ConIdInexistente_LanzaExcepcion()
    {
        // Arrange
        const int idInexistente = 999;

        // Act
        var accion = async () => await _sut.ObtenerPerfil(idInexistente);

        // Assert
        await accion.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Usuario no encontrado.");
    }

    private async Task SembrarRolAsync()
    {
        if (await _context.Roles.AnyAsync())
        {
            return;
        }

        _context.Roles.Add(new Rol
        {
            IdRol = 1,
            Nombre = "Admin",
            Descripcion = "Rol de prueba",
            Activo = true
        });

        await _context.SaveChangesAsync();
    }

    private async Task SembrarUsuarioAsync(
        string correo,
        string contrasena,
        string nombreCompleto,
        string nombreRol)
    {
        var rol = new Rol
        {
            IdRol = 1,
            Nombre = nombreRol,
            Descripcion = "Rol de prueba",
            Activo = true
        };

        _context.Roles.Add(rol);

        _context.Usuarios.Add(new Usuario
        {
            IdUsuario = 1,
            NombreCompleto = nombreCompleto,
            Cedula = "001",
            CorreoInstitucional = correo,
            ContrasenaHash = _passwordService.HashPassword(contrasena),
            IdRol = rol.IdRol,
            Rol = rol,
            Estado = "Activo"
        });

        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
