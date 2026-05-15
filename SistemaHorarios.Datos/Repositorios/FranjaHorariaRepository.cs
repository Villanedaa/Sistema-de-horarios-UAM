using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios;

public class FranjaHorariaRepository
{
    private readonly SistemaHorariosDbContext
        _context;

    public FranjaHorariaRepository(
        SistemaHorariosDbContext context)
    {
        _context = context;
    }

    // Obtiene todas las franjas horarias
    public async Task<List<FranjaHoraria>>
        ObtenerFranjasHorarias()
    {
        return await _context
            .FranjasHorarias
            .ToListAsync();
    }

    // Obtiene una franja por Id
    public async Task<FranjaHoraria?>
        ObtenerPorId(
            int idFranjaHoraria)
    {
        return await _context
            .FranjasHorarias
            .FirstOrDefaultAsync(
                f =>
                    f.IdFranjaHoraria ==
                    idFranjaHoraria
            );
    }

    // Crear franja horaria
    public async Task CrearFranjaHoraria(
        FranjaHoraria franjaHoraria)
    {
        await _context
            .FranjasHorarias
            .AddAsync(franjaHoraria);

        await _context.SaveChangesAsync();
    }

    // Actualizar franja horaria
    public async Task ActualizarFranjaHoraria(
        FranjaHoraria franjaHoraria)
    {
        _context
            .FranjasHorarias
            .Update(franjaHoraria);

        await _context.SaveChangesAsync();
    }

    // Eliminar franja horaria
    public async Task EliminarFranjaHoraria(
        FranjaHoraria franjaHoraria)
    {
        _context
            .FranjasHorarias
            .Remove(franjaHoraria);

        await _context.SaveChangesAsync();
    }
}