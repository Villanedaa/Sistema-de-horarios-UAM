using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios
{
    // Gestiona las consultas y operaciones de materias en la base de datos.
    public class MateriaRepository
    {
        private readonly SistemaHorariosDbContext contexto;

        // Recibe el contexto de base de datos.
        public MateriaRepository(SistemaHorariosDbContext contexto)
        {
            this.contexto = contexto;
        }

        // Lista todas las materias registradas.
        public async Task<List<Materia>> ListarMateriasAsync()
        {
            return await contexto.Materias.ToListAsync();
        }

        // Lista solo las materias activas.
        public async Task<List<Materia>> ListarMateriasActivasAsync()
        {
            return await contexto.Materias
                .Where(materia => materia.Activa)
                .ToListAsync();
        }

        // Busca una materia por su identificador.
        public async Task<Materia?> ObtenerMateriaPorIdAsync(int idMateria)
        {
            return await contexto.Materias
                .FirstOrDefaultAsync(materia => materia.IdMateria == idMateria);
        }

        // Busca una materia por su código.
        public async Task<Materia?> ObtenerMateriaPorCodigoAsync(string codigo)
        {
            string codigoLimpio = codigo.Trim().ToLower();

            return await contexto.Materias
                .FirstOrDefaultAsync(materia => materia.Codigo.ToLower() == codigoLimpio);
        }

        // Verifica si existe una materia con el identificador recibido.
        public async Task<bool> ExisteMateriaPorIdAsync(int idMateria)
        {
            return await contexto.Materias
                .AnyAsync(materia => materia.IdMateria == idMateria);
        }

        // Verifica si ya existe una materia con el mismo código.
        public async Task<bool> ExisteCodigoMateriaAsync(string codigo, int idMateriaExcluir)
        {
            string codigoLimpio = codigo.Trim().ToLower();

            return await contexto.Materias
                .AnyAsync(materia =>
                    materia.IdMateria != idMateriaExcluir &&
                    materia.Codigo.ToLower() == codigoLimpio
                );
        }

        // Guarda una nueva materia.
        public async Task CrearMateriaAsync(Materia materia)
        {
            await contexto.Materias.AddAsync(materia);
            await contexto.SaveChangesAsync();
        }

        // Actualiza una materia existente.
        public async Task<bool> ActualizarMateriaAsync(Materia materiaModificada)
        {
            Materia? materiaActual = await ObtenerMateriaPorIdAsync(materiaModificada.IdMateria);

            if (materiaActual == null)
            {
                return false;
            }

            materiaActual.Codigo = materiaModificada.Codigo;
            materiaActual.Nombre = materiaModificada.Nombre;
            materiaActual.Creditos = materiaModificada.Creditos;
            materiaActual.IntensidadHorariaSemanal = materiaModificada.IntensidadHorariaSemanal;
            materiaActual.Semestre = materiaModificada.Semestre;
            materiaActual.Activa = materiaModificada.Activa;

            await contexto.SaveChangesAsync();

            return true;
        }

        // Desactiva una materia sin eliminarla de la base de datos.
        public async Task<bool> DesactivarMateriaAsync(int idMateria)
        {
            Materia? materia = await ObtenerMateriaPorIdAsync(idMateria);

            if (materia == null)
            {
                return false;
            }

            materia.Activa = false;

            await contexto.SaveChangesAsync();

            return true;
        }
    }
}