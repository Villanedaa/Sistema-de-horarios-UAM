namespace SistemaHorarios.Modelos.DTOs.Reportes;

// Resume los horarios activos generados por grupo académico.
public class ReporteHorarioGeneradoDto
{
    public int IdGrupo { get; set; }
    public string CodigoGrupo { get; set; } = string.Empty;
    public string NombreGrupo { get; set; } = string.Empty;
    public string Jornada { get; set; } = string.Empty;
    public int NumeroSemestre { get; set; }
    public int TotalMaterias { get; set; }
    public int TotalDocentes { get; set; }
    public int TotalBloques { get; set; }
    public int TotalHoras { get; set; }
    public string Estado { get; set; } = string.Empty;
}
