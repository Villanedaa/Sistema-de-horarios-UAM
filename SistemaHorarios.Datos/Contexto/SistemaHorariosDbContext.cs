using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Contexto
{
    /// <summary>
    /// Contexto principal de base de datos del sistema.
    ///
    /// Esta clase es responsable de:
    /// - gestionar la conexión con MySQL,
    /// - mapear entidades a tablas,
    /// - configurar relaciones,
    /// - ejecutar migraciones,
    /// - manejar persistencia mediante Entity Framework Core.
    ///
    /// Actúa como puente entre:
    /// lógica de negocio
    /// y
    /// base de datos.
    /// </summary>
    public class SistemaHorariosDbContext : DbContext
    {
        /// <summary>
        /// Constructor del DbContext.
        ///
        /// Recibe las configuraciones de conexión
        /// definidas en Program.cs.
        /// </summary>
        /// <param name="options">
        /// Opciones de configuración del contexto.
        /// </param>
        public SistemaHorariosDbContext(
            DbContextOptions<SistemaHorariosDbContext> options
        ) : base(options)
        {
        }

        /// <summary>
        /// Permite configurar el modelo de Entity Framework.
        ///
        /// Aquí se pueden:
        /// - definir relaciones,
        /// - restricciones,
        /// - seeds,
        /// - configuraciones avanzadas.
        /// </summary>
        /// <param name="modelBuilder">
        /// Constructor del modelo de Entity Framework.
        /// </param>
        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Representa la tabla Usuarios en la base de datos.
        /// </summary>
        public DbSet<Usuario> Usuarios { get; set; }

        /// <summary>
        /// Representa la tabla Roles en la base de datos.
        /// </summary>
        public DbSet<Rol> Roles { get; set; }

        /// <summary>
        /// Representa la tabla Materias en la base de datos.
        /// </summary>
        public DbSet<Materia> Materias { get; set; }

        /// <summary>
        /// Representa la tabla Prerrequisitos
        /// en la base de datos.
        /// </summary>
        public DbSet<Prerrequisito> Prerrequisitos { get; set; }
    }
}