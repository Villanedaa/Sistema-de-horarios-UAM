using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Seed;

public static class SeedData
{
    public static void SeedRoles(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>().HasData(
            new Rol
            {
                IdRol = 1,
                Nombre = "Administrador"
            },

            new Rol
            {
                IdRol = 2,
                Nombre = "Coordinador"
            },

            new Rol
            {
                IdRol = 3,
                Nombre = "Docente"
            }
        );
    }
}