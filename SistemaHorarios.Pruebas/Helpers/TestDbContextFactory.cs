using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;

namespace SistemaHorarios.Pruebas.Helpers;

public static class TestDbContextFactory
{
    public static SistemaHorariosDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<SistemaHorariosDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SistemaHorariosDbContext(opciones);
    }
}
