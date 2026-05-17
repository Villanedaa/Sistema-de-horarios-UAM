using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly SistemaHorariosDbContext _context;

    public UsuarioRepository(SistemaHorariosDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario> CrearAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();

        return usuario;
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id)
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.IdUsuario == id);
    }

    public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
    {
        return await _context.Usuarios
            .Include(u => u.Rol)
            .ToListAsync();
    }

    public async Task<bool> ActualizarAsync(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);

        int filasAfectadas =
            await _context.SaveChangesAsync();

        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarAsync(Usuario usuario)
    {
        _context.Usuarios.Remove(usuario);

        int filasAfectadas =
            await _context.SaveChangesAsync();

        return filasAfectadas > 0;
    }

    public async Task<bool> ExisteCorreoAsync(string correo)
    {
        return await _context.Usuarios
            .AnyAsync(u =>
                u.CorreoInstitucional == correo);
    }

    public async Task<bool> ExisteCedulaAsync(string cedula)
    {
        return await _context.Usuarios
            .AnyAsync(u =>
                u.Cedula == cedula);
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }
}