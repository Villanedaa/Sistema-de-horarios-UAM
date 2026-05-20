using FluentAssertions;
using SistemaHorarios.Logica.Negocio.Horarios;
using SistemaHorarios.Modelos.Entidades;
using Xunit;

namespace SistemaHorarios.Pruebas.Horarios;

public class ValidadorHorarioTests
{
    private readonly ValidadorHorario _sut = new();

    [Fact]
    public void Validar_ConHorarioNull_RetornaError()
    {
        // Arrange
        Horario? horario = null;

        // Act
        var errores = _sut.Validar(horario!);

        // Assert
        errores.Should().Contain("El horario no puede estar vacío.");
    }

    [Fact]
    public void Validar_ConDatosValidos_NoRetornaErrores()
    {
        // Arrange
        var horario = CrearHorarioValido();

        // Act
        var errores = _sut.Validar(horario);

        // Assert
        errores.Should().BeEmpty();
    }

    [Fact]
    public void Validar_ConIdGrupoInvalido_RetornaError()
    {
        // Arrange
        var horario = CrearHorarioValido();
        horario.IdGrupo = 0;

        // Act
        var errores = _sut.Validar(horario);

        // Assert
        errores.Should().Contain("El grupo no es válido.");
    }

    [Fact]
    public void Validar_ConIdDocenteInvalido_RetornaError()
    {
        // Arrange
        var horario = CrearHorarioValido();
        horario.IdDocente = -1;

        // Act
        var errores = _sut.Validar(horario);

        // Assert
        errores.Should().Contain("El docente no es válido.");
    }

    [Fact]
    public void Validar_ConIdFranjaHorariaInvalida_RetornaError()
    {
        // Arrange
        var horario = CrearHorarioValido();
        horario.IdFranjaHoraria = 0;

        // Act
        var errores = _sut.Validar(horario);

        // Assert
        errores.Should().Contain("La franja horaria no es válida.");
    }

    private static Horario CrearHorarioValido()
    {
        return new Horario
        {
            IdGrupo = 1,
            IdMateria = 1,
            IdDocente = 1,
            IdFranjaHoraria = 1
        };
    }
}
