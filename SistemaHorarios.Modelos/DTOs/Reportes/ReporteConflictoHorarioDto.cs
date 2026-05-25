namespace SistemaHorarios.Modelos.DTOs.Reportes;

// Describe una inconsistencia detectada en los horarios generados.
public class ReporteConflictoHorarioDto
{
    public string TipoConflicto { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Grupo { get; set; } = string.Empty;
    public string Docente { get; set; } = string.Empty;
    public string Materia { get; set; } = string.Empty;
    public string DiaSemana { get; set; } = string.Empty;
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFin { get; set; } = string.Empty;
}
