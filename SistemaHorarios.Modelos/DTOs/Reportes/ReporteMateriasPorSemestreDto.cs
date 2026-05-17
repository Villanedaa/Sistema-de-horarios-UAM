namespace SistemaHorarios.Modelos.DTOs.Reportes;

public class ReporteMateriasPorSemestreDto
{
    public int Semestre { get; set; }

    public int TotalMaterias { get; set; }

    public List<string> Materias { get; set; } =
        new List<string>();
}