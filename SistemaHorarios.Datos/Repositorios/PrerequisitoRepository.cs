using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios
{
    // Gestiona las consultas y operaciones de prerrequisitos en la base de datos.
    public class PrerrequisitoRepository
    {
        private readonly SistemaHorariosDbContext contexto;

        // Recibe el contexto de base de datos.
        public PrerrequisitoRepository(SistemaHorariosDbContext contexto)
        {
            this.contexto = contexto;
        }

        // Lista todos los prerrequisitos registrados.
        public async Task<List<Prerrequisito>> ListarPrerrequisitosAsync()
        {
            return await contexto.Prerrequisitos.ToListAsync();
        }

        // Lista solo los prerrequisitos activos.
        public async Task<List<Prerrequisito>> ListarPrerrequisitosActivosAsync()
        {
            return await contexto.Prerrequisitos
                .Where(prerrequisito => prerrequisito.Activo)
                .ToListAsync();
        }

        // Busca un prerrequisito por su identificador.
        public async Task<Prerrequisito?> ObtenerPrerrequisitoPorIdAsync(int idPrerrequisito)
        {
            return await contexto.Prerrequisitos
                .FirstOrDefaultAsync(prerrequisito => prerrequisito.IdPrerrequisito == idPrerrequisito);
        }

        // Lista los prerrequisitos activos de una materia.
        public async Task<List<Prerrequisito>> ListarPrerrequisitosPorMateriaAsync(int idMateria)
        {
            return await contexto.Prerrequisitos
                .Where(prerrequisito =>
                    prerrequisito.IdMateria == idMateria &&
                    prerrequisito.Activo
                )
                .ToListAsync();
        }

        // Verifica si ya existe un prerrequisito activo entre dos materias.
        public async Task<bool> ExistePrerrequisitoActivoAsync(int idMateria, int idMateriaPrerrequisito)
        {
            return await contexto.Prerrequisitos
                .AnyAsync(prerrequisito =>
                    prerrequisito.IdMateria == idMateria &&
                    prerrequisito.IdMateriaPrerrequisito == idMateriaPrerrequisito &&
                    prerrequisito.Activo
                );
        }

        // Verifica si existe una relación circular directa.
        public async Task<bool> ExisteRelacionCircularDirectaAsync(int idMateria, int idMateriaPrerrequisito)
        {
            return await contexto.Prerrequisitos
                .AnyAsync(prerrequisito =>
                    prerrequisito.IdMateria == idMateriaPrerrequisito &&
                    prerrequisito.IdMateriaPrerrequisito == idMateria &&
                    prerrequisito.Activo
                );
        }

        // Guarda un nuevo prerrequisito.
        public async Task CrearPrerrequisitoAsync(Prerrequisito prerrequisito)
        {
            await contexto.Prerrequisitos.AddAsync(prerrequisito);
            await contexto.SaveChangesAsync();
        }

        // Desactiva un prerrequisito sin eliminarlo de la base de datos.
        public async Task<bool> DesactivarPrerrequisitoAsync(int idPrerrequisito)
        {
            Prerrequisito? prerrequisito = await ObtenerPrerrequisitoPorIdAsync(idPrerrequisito);

            if (prerrequisito == null)
            {
                return false;
            }

            prerrequisito.Activo = false;

            await contexto.SaveChangesAsync();

            return true;
        }
    }
}