using FluentAssertions;
using SistemaHorarios.Logica.Negocio.Materias;
using SistemaHorarios.Modelos.Entidades;
using Xunit;

namespace SistemaHorarios.Pruebas.Materias;

public class ValidadorMateriaTests
{
    private readonly ValidadorMateria _sut = new();

    [Fact]
    public void Validar_ConMateriaNull_RetornaError()
    {
        // Arrange
        Materia? materia = null;

        // Act
        var errores = _sut.Validar(materia!);

        // Assert
        errores.Should().Contain("La materia no puede estar vacía.");
    }

    [Fact]
    public void Validar_ConDatosValidos_NoRetornaErrores()
    {
        // Arrange
        var materia = CrearMateriaValida();

        // Act
        var errores = _sut.Validar(materia);

        // Assert
        errores.Should().BeEmpty();
    }

    [Fact]
    public void Validar_ConCodigoVacio_RetornaErrorDeCodigo()
    {
        // Arrange
        var materia = CrearMateriaValida();
        materia.Codigo = "   ";

        // Act
        var errores = _sut.Validar(materia);

        // Assert
        errores.Should().Contain("El código de la materia es obligatorio.");
    }

    [Fact]
    public void Validar_ConCreditosCero_RetornaErrorDeCreditos()
    {
        // Arrange
        var materia = CrearMateriaValida();
        materia.Creditos = 0;

        // Act
        var errores = _sut.Validar(materia);

        // Assert
        errores.Should().Contain("El número de créditos debe ser mayor a cero.");
    }

    [Fact]
    public void Validar_ConCantidadGruposNegativa_RetornaError()
    {
        // Arrange
        var materia = CrearMateriaValida();
        materia.CantidadGrupos = -1;

        // Act
        var errores = _sut.Validar(materia);

        // Assert
        errores.Should().Contain("La cantidad de grupos no puede ser negativa.");
    }

    private static Materia CrearMateriaValida()
    {
        return new Materia
        {
            Codigo = "MAT101",
            Nombre = "Cálculo I",
            Creditos = 4,
            IntensidadHorariaSemanal = 6,
            Semestre = 2,
            CantidadGrupos = 2
        };
    }
}
