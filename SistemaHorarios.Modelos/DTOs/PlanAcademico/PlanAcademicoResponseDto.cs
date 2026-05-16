namespace SistemaHorarios.Modelos.DTOs.PlanAcademico;

public class PlanAcademicoResponseDto
{
    public int IdPlanAcademico { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Programa { get; set; } = string.Empty;

    public int Anio { get; set; }

    public string Estado { get; set; } = string.Empty;

    public List<SemestrePlanResponseDto> Semestres { get; set; } =
        new List<SemestrePlanResponseDto>();
}