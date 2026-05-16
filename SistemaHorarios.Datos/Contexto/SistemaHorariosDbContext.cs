using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Contexto;

public class SistemaHorariosDbContext : DbContext
{
    public SistemaHorariosDbContext(
        DbContextOptions<SistemaHorariosDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }

    public DbSet<Rol> Roles { get; set; }

    public DbSet<Materia> Materias { get; set; }

    public DbSet<Prerrequisito> Prerrequisitos { get; set; }

    public DbSet<FranjaHoraria> FranjasHorarias { get; set; }

    public DbSet<Grupo> Grupos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurarUsuario(modelBuilder);

        ConfigurarRoles(modelBuilder);
    }

    private void ConfigurarUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasOne(usuario => usuario.Rol)
            .WithMany()
            .HasForeignKey(usuario => usuario.IdRol);
    }

    private void ConfigurarRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>().HasData(
            new Rol
            {
                IdRol = 1,
                Nombre = "Administrador",
                Activo = true
            },
            new Rol
            {
                IdRol = 2,
                Nombre = "Coordinador",
                Activo = true
            },
            new Rol
            {
                IdRol = 3,
                Nombre = "Docente",
                Activo = true
            }
        );
    }
}