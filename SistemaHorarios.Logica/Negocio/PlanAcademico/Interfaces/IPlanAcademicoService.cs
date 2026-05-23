using SistemaHorarios.Modelos.DTOs.PlanAcademico;

namespace SistemaHorarios.Logica.Negocio.PlanAcademico.Interfaces;

public interface IPlanAcademicoService
{
    Task<List<PlanAcademicoResponseDto>> ObtenerTodosAsync();

    Task<PlanAcademicoResponseDto?> ObtenerPorIdAsync(
        int idPlanAcademico);

    Task<PlanAcademicoResponseDto> CrearAsync(
        CrearPlanAcademicoDto dto);

    Task<bool> ActualizarAsync(
        int idPlanAcademico,
        ActualizarPlanAcademicoDto dto);

    Task<bool> EliminarAsync(
        int idPlanAcademico);

    Task<bool> ActivarAsync(
        int idPlanAcademico);

    Task<SemestrePlanResponseDto> AgregarSemestreAsync(
        int idPlanAcademico,
        CrearSemestrePlanDto dto);

    Task<MateriaPlanResponseDto> AgregarMateriaASemestreAsync(
        int idSemestrePlan,
        AgregarMateriaPlanDto dto);

    Task<bool> EliminarMateriaDelPlanAsync(
        int idMateriaPlan);
}