using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Contexto
{
    public class SistemaHorariosDbContext : DbContext
    {
        public SistemaHorariosDbContext(
            DbContextOptions<SistemaHorariosDbContext> options
        ) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Rol> Roles { get; set; }
    }
}