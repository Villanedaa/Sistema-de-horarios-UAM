using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.API.Extensiones;

public static class FranjasHorariasSeeder
{
    private static readonly string[] Dias =
    [
        "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"
    ];

    // Franjas en formato (HoraInicio, HoraFin) — excluye bloques institucionales
    private static readonly (TimeSpan Inicio, TimeSpan Fin)[] Franjas =
    [
        (new TimeSpan(7,  0, 0), new TimeSpan(8,  0, 0)),
        (new TimeSpan(8,  0, 0), new TimeSpan(9,  0, 0)),
        (new TimeSpan(9,  0, 0), new TimeSpan(10, 0, 0)),
        (new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0)),
        (new TimeSpan(11, 0, 0), new TimeSpan(12, 0, 0)),
        (new TimeSpan(14, 0, 0), new TimeSpan(15, 0, 0)),
        (new TimeSpan(15, 0, 0), new TimeSpan(16, 0, 0)),
        (new TimeSpan(16, 0, 0), new TimeSpan(17, 0, 0)),
        (new TimeSpan(17, 0, 0), new TimeSpan(18, 0, 0)),
        (new TimeSpan(18, 30, 0), new TimeSpan(19, 30, 0)),
        (new TimeSpan(19, 30, 0), new TimeSpan(20, 30, 0)),
        (new TimeSpan(20, 30, 0), new TimeSpan(21, 30, 0)),
        (new TimeSpan(21, 30, 0), new TimeSpan(22, 30, 0)),
    ];

    public static async Task SembrarAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var contexto = scope.ServiceProvider.GetRequiredService<SistemaHorariosDbContext>();

        bool hayFranjas = await contexto.FranjasHorarias.AnyAsync();
        if (hayFranjas) return;

        var franjas = new List<FranjaHoraria>();

        foreach (var dia in Dias)
        {
            foreach (var (inicio, fin) in Franjas)
            {
                franjas.Add(new FranjaHoraria
                {
                    DiaSemana = dia,
                    HoraInicio = inicio,
                    HoraFin = fin,
                    Activa = true
                });
            }
        }

        await contexto.FranjasHorarias.AddRangeAsync(franjas);
        await contexto.SaveChangesAsync();
    }
}
