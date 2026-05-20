using FluentAssertions;
using SistemaHorarios.Logica.Negocio.Grupos;
using SistemaHorarios.Modelos.Entidades;
using Xunit;

namespace SistemaHorarios.Pruebas.Grupos;

public class ValidadorGrupoTests
{
    private readonly ValidadorGrupo _sut = new();

    [Fact]
    public void Validar_ConGrupoNull_RetornaError()
    {
        // Arrange
        Grupo? grupo = null;

        // Act
        var errores = _sut.Validar(grupo!);

        // Assert
        errores.Should().Contain("El grupo no puede estar vacío.");
    }

    [Fact]
    public void Validar_ConDatosValidos_NoRetornaErrores()
    {
        // Arrange
        var grupo = CrearGrupoValido();

        // Act
        var errores = _sut.Validar(grupo);

        // Assert
        errores.Should().BeEmpty();
    }

    [Fact]
    public void Validar_ConCodigoVacio_RetornaErrorDeCodigo()
    {
        // Arrange
        var grupo = CrearGrupoValido();
        grupo.Codigo = "";

        // Act
        var errores = _sut.Validar(grupo);

        // Assert
        errores.Should().Contain("El código del grupo es obligatorio.");
    }

    [Fact]
    public void Validar_ConNumeroSemestreCero_RetornaError()
    {
        // Arrange
        var grupo = CrearGrupoValido();
        grupo.NumeroSemestre = 0;

        // Act
        var errores = _sut.Validar(grupo);

        // Assert
        errores.Should().Contain("El número de semestre debe ser mayor a cero.");
    }

    [Fact]
    public void Validar_ConPlanAcademicoInvalido_RetornaError()
    {
        // Arrange
        var grupo = CrearGrupoValido();
        grupo.IdPlanAcademico = 0;

        // Act
        var errores = _sut.Validar(grupo);

        // Assert
        errores.Should().Contain("El plan académico asociado no es válido.");
    }

    private static Grupo CrearGrupoValido()
    {
        return new Grupo
        {
            Codigo = "G-101",
            Nombre = "Grupo A",
            Jornada = "Diurna",
            TipoGrupo = "Regular",
            NumeroSemestre = 1,
            CantidadEstudiantes = 30,
            IdPlanAcademico = 1
        };
    }
}
