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

    public DbSet<PlanAcademico> PlanesAcademicos { get; set; }

    public DbSet<SemestrePlan> SemestresPlan { get; set; }

    public DbSet<MateriaPlan> MateriasPlan { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurarUsuario(modelBuilder);

        ConfigurarPlanAcademico(modelBuilder);

        ConfigurarRoles(modelBuilder);
    }

    private void ConfigurarUsuario(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>()
            .HasOne(usuario => usuario.Rol)
            .WithMany()
            .HasForeignKey(usuario => usuario.IdRol);
    }

    private void ConfigurarPlanAcademico(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlanAcademico>()
            .HasKey(plan => plan.IdPlanAcademico);

        modelBuilder.Entity<SemestrePlan>()
            .HasKey(semestre => semestre.IdSemestrePlan);

        modelBuilder.Entity<MateriaPlan>()
            .HasKey(materiaPlan => materiaPlan.IdMateriaPlan);

        modelBuilder.Entity<SemestrePlan>()
            .HasOne(semestre => semestre.PlanAcademico)
            .WithMany(plan => plan.Semestres)
            .HasForeignKey(semestre => semestre.IdPlanAcademico);

        modelBuilder.Entity<MateriaPlan>()
            .HasOne(materiaPlan => materiaPlan.SemestrePlan)
            .WithMany(semestre => semestre.MateriasPlan)
            .HasForeignKey(materiaPlan => materiaPlan.IdSemestrePlan);

        modelBuilder.Entity<MateriaPlan>()
            .HasOne(materiaPlan => materiaPlan.Materia)
            .WithMany()
            .HasForeignKey(materiaPlan => materiaPlan.IdMateria);

        modelBuilder.Entity<SemestrePlan>()
            .HasIndex(semestre =>
                new
                {
                    semestre.IdPlanAcademico,
                    semestre.NumeroSemestre
                })
            .IsUnique();

        modelBuilder.Entity<MateriaPlan>()
            .HasIndex(materiaPlan =>
                new
                {
                    materiaPlan.IdSemestrePlan,
                    materiaPlan.IdMateria
                })
            .IsUnique();
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