using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios;

public class RolRepository
{
    private readonly SistemaHorariosDbContext _context;

    public RolRepository(
        SistemaHorariosDbContext context)
    {
        _context = context;
    }

    // Obtiene todos los roles
    public async Task<List<Rol>> ObtenerRoles()
    {
        return await _context.Roles.ToListAsync();
    }

    // Obtiene un rol por Id
    public async Task<Rol?> ObtenerPorId(
        int idRol)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(
                r => r.IdRol == idRol
            );
    }

    // Obtiene un rol por nombre
    public async Task<Rol?> ObtenerPorNombre(
        string nombre)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(
                r => r.Nombre == nombre
            );
    }

    // Crea un nuevo rol
    public async Task CrearRol(
        Rol rol)
    {
        await _context.Roles.AddAsync(rol);

        await _context.SaveChangesAsync();
    }

    // Actualiza un rol existente
    public async Task ActualizarRol(
        Rol rol)
    {
        _context.Roles.Update(rol);

        await _context.SaveChangesAsync();
    }

    // Elimina un rol
    public async Task EliminarRol(
        Rol rol)
    {
        _context.Roles.Remove(rol);

        await _context.SaveChangesAsync();
    }
}