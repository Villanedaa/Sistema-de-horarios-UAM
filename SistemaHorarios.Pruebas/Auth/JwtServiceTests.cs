using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Modelos.Entidades;
using SistemaHorarios.Pruebas.Helpers;
using Xunit;

namespace SistemaHorarios.Pruebas.Auth;

public class JwtServiceTests
{
    private readonly JwtService _sut = TestConfigurationFactory.CrearJwtService();

    [Fact]
    public void GenerarToken_ConUsuarioValido_RetornaJwtNoVacio()
    {
        // Arrange
        var usuario = CrearUsuarioConRol();

        // Act
        var token = _sut.GenerarToken(usuario);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void GenerarToken_ConUsuarioValido_IncluyeClaimsEsperados()
    {
        // Arrange
        var usuario = CrearUsuarioConRol();

        // Act
        var token = _sut.GenerarToken(usuario);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        // Assert
        jwt.Claims.Should().Contain(c =>
            c.Type == ClaimTypes.NameIdentifier && c.Value == "1");
        jwt.Claims.Should().Contain(c =>
            c.Type == ClaimTypes.Email && c.Value == "juan@uam.edu");
        jwt.Claims.Should().Contain(c =>
            c.Type == ClaimTypes.Name && c.Value == "Juan Pérez");
        jwt.Claims.Should().Contain(c =>
            c.Type == ClaimTypes.Role && c.Value == "Admin");
    }

    private static Usuario CrearUsuarioConRol()
    {
        return new Usuario
        {
            IdUsuario = 1,
            NombreCompleto = "Juan Pérez",
            CorreoInstitucional = "juan@uam.edu",
            IdRol = 1,
            Rol = new Rol
            {
                IdRol = 1,
                Nombre = "Admin"
            }
        };
    }
}
