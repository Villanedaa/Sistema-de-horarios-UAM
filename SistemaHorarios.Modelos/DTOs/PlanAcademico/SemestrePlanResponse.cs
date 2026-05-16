namespace SistemaHorarios.Modelos.DTOs.PlanAcademico;

public class SemestrePlanResponseDto
{
    public int IdSemestrePlan { get; set; }

    public int NumeroSemestre { get; set; }

    public List<MateriaPlanResponseDto> Materias { get; set; } =
        new List<MateriaPlanResponseDto>();
}