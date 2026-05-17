using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios;

public class DisponibilidadDocenteRepository
    : IDisponibilidadDocenteRepository
{
    private readonly SistemaHorariosDbContext _context;

    public DisponibilidadDocenteRepository(
        SistemaHorariosDbContext context)
    {
        _context = context;
    }

    public async Task<List<DisponibilidadDocente>>
        ObtenerPorDocenteAsync(int idDocente)
    {
        return await _context.DisponibilidadesDocentes
            .Where(x => x.IdDocente == idDocente)
            .ToListAsync();
    }

    public async Task EliminarPorDocenteAsync(int idDocente)
    {
        var disponibilidades = await _context.DisponibilidadesDocentes
            .Where(x => x.IdDocente == idDocente)
            .ToListAsync();

        _context.DisponibilidadesDocentes.RemoveRange(disponibilidades);

        await _context.SaveChangesAsync();
    }

    public async Task CrearRangoAsync(
        List<DisponibilidadDocente> disponibilidades)
    {
        await _context.DisponibilidadesDocentes
            .AddRangeAsync(disponibilidades);

        await _context.SaveChangesAsync();
    }
}