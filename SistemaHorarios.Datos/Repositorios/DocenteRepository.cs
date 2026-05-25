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

public class DocenteRepository : IDocenteRepository
{
    private readonly SistemaHorariosDbContext _context;

    public DocenteRepository(SistemaHorariosDbContext context)
    {
        _context = context;
    }

    public async Task<List<Docente>> ObtenerTodosAsync()
    {
        return await _context.Docentes.ToListAsync();
    }

    public async Task<Docente?> ObtenerPorIdAsync(int id)
    {
        return await _context.Docentes
            .FirstOrDefaultAsync(x => x.IdDocente == id);
    }

    public async Task<Docente> CrearAsync(Docente docente)
    {
        _context.Docentes.Add(docente);

        await _context.SaveChangesAsync();

        return docente;
    }

    public async Task ActualizarAsync(Docente docente)
    {
        _context.Docentes.Update(docente);

        await _context.SaveChangesAsync();
    }
	

    public async Task EliminarAsync(Docente docente)
    {
    	docente.Activo = false;

    	_context.Docentes.Update(docente);

    	await _context.SaveChangesAsync();
    }

    public async Task ActivarAsync(Docente docente)
    {
    	docente.Activo = true;

    	_context.Docentes.Update(docente);

    	await _context.SaveChangesAsync();
    }
    
    
    public async Task<bool> ExistePorIdAsync(int id)
    {
        return await _context.Docentes
            .AnyAsync(x => x.IdDocente == id);
    }

    public async Task ActualizarMateriasAsync(int idDocente, List<int> idsMateria)
    {
        var existentes = await _context.DocenteMaterias
            .Where(dm => dm.IdDocente == idDocente)
            .ToListAsync();

        _context.DocenteMaterias.RemoveRange(existentes);

        foreach (var idMateria in idsMateria)
        {
            _context.DocenteMaterias.Add(new DocenteMateria
            {
                IdDocente = idDocente,
                IdMateria = idMateria,
                Activo = true
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<string>> ObtenerNombresMateriasAsync(int idDocente)
    {
        return await _context.DocenteMaterias
            .Where(dm => dm.IdDocente == idDocente && dm.Activo)
            .Include(dm => dm.Materia)
            .Select(dm => dm.Materia!.Nombre)
            .ToListAsync();
    }
}