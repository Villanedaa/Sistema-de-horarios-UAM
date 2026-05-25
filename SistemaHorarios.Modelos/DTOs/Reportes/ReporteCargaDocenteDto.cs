namespace SistemaHorarios.Modelos.DTOs.Reportes;

// Resume la carga semanal asignada a un docente.
public class ReporteCargaDocenteDto
{
    public int IdDocente { get; set; }
    public string Docente { get; set; } = string.Empty;
    public string EstadoDocente { get; set; } = string.Empty;
    public int CantidadMaterias { get; set; }
    public int CantidadGrupos { get; set; }
    public int BloquesSemanales { get; set; }
    public int HorasSemanales { get; set; }
}
