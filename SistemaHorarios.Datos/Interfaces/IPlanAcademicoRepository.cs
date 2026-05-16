using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Interfaces;

public interface IPlanAcademicoRepository
{
    Task<List<PlanAcademico>> ObtenerTodosAsync();

    Task<PlanAcademico?> ObtenerPorIdAsync(int idPlanAcademico);

    Task CrearAsync(PlanAcademico planAcademico);

    Task<bool> ActualizarAsync(PlanAcademico planAcademico);

    Task<bool> EliminarAsync(PlanAcademico planAcademico);

    Task<SemestrePlan?> ObtenerSemestrePorIdAsync(int idSemestrePlan);

    Task<SemestrePlan?> ObtenerSemestrePorNumeroAsync(
        int idPlanAcademico,
        int numeroSemestre);

    Task CrearSemestreAsync(SemestrePlan semestrePlan);

    Task<Materia?> ObtenerMateriaPorIdAsync(int idMateria);

    Task<bool> ExisteMateriaEnSemestreAsync(
        int idSemestrePlan,
        int idMateria);

    Task CrearMateriaPlanAsync(MateriaPlan materiaPlan);

    Task<MateriaPlan?> ObtenerMateriaPlanPorIdAsync(int idMateriaPlan);

    Task<bool> EliminarMateriaPlanAsync(MateriaPlan materiaPlan);
}