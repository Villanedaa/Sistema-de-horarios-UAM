using FluentAssertions;
using SistemaHorarios.Logica.Negocio.Auth;
using Xunit;

namespace SistemaHorarios.Pruebas.Auth;

public class PasswordServiceTests
{
    private readonly PasswordService _sut = new();

    [Fact]
    public void HashPassword_DebeGenerarHashDistintoAlTextoPlano()
    {
        // Arrange
        const string contrasena = "MiClave123";

        // Act
        var hash = _sut.HashPassword(contrasena);

        // Assert
        hash.Should().NotBe(contrasena);
    }

    [Fact]
    public void VerifyPassword_ConClaveCorrecta_DebeRetornarTrue()
    {
        // Arrange
        const string contrasena = "MiClave123";
        var hash = _sut.HashPassword(contrasena);

        // Act
        var resultado = _sut.VerifyPassword(contrasena, hash);

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_ConClaveIncorrecta_DebeRetornarFalse()
    {
        // Arrange
        var hash = _sut.HashPassword("clave-correcta");

        // Act
        var resultado = _sut.VerifyPassword("clave-incorrecta", hash);

        // Assert
        resultado.Should().BeFalse();
    }
}
