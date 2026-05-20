using FluentAssertions;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Logica.Negocio.Materias;
using SistemaHorarios.Modelos.Entidades;
using SistemaHorarios.Pruebas.Helpers;
using Xunit;

namespace SistemaHorarios.Pruebas.Materias;

public class GestorMateriaTests : IDisposable
{
    private readonly SistemaHorariosDbContext _context;
    private readonly GestorMateria _sut;

    public GestorMateriaTests()
    {
        _context = TestDbContextFactory.CrearContexto();
        var repositorio = new MateriaRepository(_context);
        _sut = new GestorMateria(repositorio);
    }

    [Fact]
    public async Task ConsultarMateriaPorIdAsync_ConIdCero_RetornaNull()
    {
        // Arrange
        const int idInvalido = 0;

        // Act
        var resultado = await _sut.ConsultarMateriaPorIdAsync(idInvalido);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task CrearMateriaAsync_ConCodigoDuplicado_RetornaError()
    {
        // Arrange
        await SembrarMateriaAsync("MAT101", "Cálculo I");

        var materiaDuplicada = new Materia
        {
            Codigo = "MAT101",
            Nombre = "Otra materia",
            Creditos = 3,
            IntensidadHorariaSemanal = 4,
            Semestre = 1,
            CantidadGrupos = 1
        };

        // Act
        var errores = await _sut.CrearMateriaAsync(materiaDuplicada);

        // Assert
        errores.Should().Contain("Ya existe una materia registrada con el mismo código.");
    }

    [Fact]
    public async Task CrearMateriaAsync_ConDatosValidos_PersisteMateria()
    {
        // Arrange
        var materiaNueva = new Materia
        {
            Codigo = " MAT201 ",
            Nombre = " Álgebra ",
            Creditos = 3,
            IntensidadHorariaSemanal = 5,
            Semestre = 2,
            CantidadGrupos = 2
        };

        // Act
        var errores = await _sut.CrearMateriaAsync(materiaNueva);

        // Assert
        errores.Should().BeEmpty();

        var materiaGuardada = await _sut.ConsultarMateriaPorCodigoAsync("MAT201");
        materiaGuardada.Should().NotBeNull();
        materiaGuardada!.Nombre.Should().Be("Álgebra");
        materiaGuardada.Activa.Should().BeTrue();
    }

    [Fact]
    public async Task DesactivarMateriaAsync_ConIdInexistente_RetornaError()
    {
        // Arrange
        const int idInexistente = 999;

        // Act
        var errores = await _sut.DesactivarMateriaAsync(idInexistente);

        // Assert
        errores.Should().Contain("La materia que desea desactivar no existe.");
    }

    private async Task SembrarMateriaAsync(string codigo, string nombre)
    {
        _context.Materias.Add(new Materia
        {
            Codigo = codigo,
            Nombre = nombre,
            Creditos = 4,
            IntensidadHorariaSemanal = 6,
            Semestre = 1,
            CantidadGrupos = 1,
            Activa = true
        });

        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
