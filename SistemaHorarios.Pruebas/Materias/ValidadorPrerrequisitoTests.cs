using FluentAssertions;
using SistemaHorarios.Logica.Negocio.Materias;
using SistemaHorarios.Modelos.Entidades;
using Xunit;

namespace SistemaHorarios.Pruebas.Materias;

public class ValidadorPrerrequisitoTests
{
    private readonly ValidadorPrerrequisito _sut = new();

    [Fact]
    public void Validar_ConPrerrequisitoNull_RetornaError()
    {
        // Arrange
        Prerrequisito? prerrequisito = null;

        // Act
        var errores = _sut.Validar(prerrequisito!);

        // Assert
        errores.Should().Contain("El prerrequisito no puede estar vacío.");
    }

    [Fact]
    public void Validar_ConDatosValidos_NoRetornaErrores()
    {
        // Arrange
        var prerrequisito = CrearPrerrequisitoValido();

        // Act
        var errores = _sut.Validar(prerrequisito);

        // Assert
        errores.Should().BeEmpty();
    }

    [Fact]
    public void Validar_ConMismaMateriaComoPrerrequisito_RetornaError()
    {
        // Arrange
        var prerrequisito = new Prerrequisito
        {
            IdMateria = 5,
            IdMateriaPrerrequisito = 5
        };

        // Act
        var errores = _sut.Validar(prerrequisito);

        // Assert
        errores.Should().Contain("Una materia no puede ser prerrequisito de sí misma.");
    }

    [Fact]
    public void Validar_ConIdMateriaPrincipalInvalido_RetornaError()
    {
        // Arrange
        var prerrequisito = new Prerrequisito
        {
            IdMateria = 0,
            IdMateriaPrerrequisito = 2
        };

        // Act
        var errores = _sut.Validar(prerrequisito);

        // Assert
        errores.Should().Contain("La materia principal no es válida.");
    }

    [Fact]
    public void Validar_ConIdMateriaPrerrequisitoInvalido_RetornaError()
    {
        // Arrange
        var prerrequisito = new Prerrequisito
        {
            IdMateria = 1,
            IdMateriaPrerrequisito = -1
        };

        // Act
        var errores = _sut.Validar(prerrequisito);

        // Assert
        errores.Should().Contain("La materia prerrequisito no es válida.");
    }

    private static Prerrequisito CrearPrerrequisitoValido()
    {
        return new Prerrequisito
        {
            IdMateria = 1,
            IdMateriaPrerrequisito = 2
        };
    }
}
