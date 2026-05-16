namespace SistemaHorarios.Modelos.DTOs.Reportes;

public class ReportePlanAcademicoDto
{
    public int IdPlanAcademico { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Programa { get; set; } = string.Empty;

    public int Anio { get; set; }

    public int TotalSemestres { get; set; }

    public int TotalMaterias { get; set; }
}