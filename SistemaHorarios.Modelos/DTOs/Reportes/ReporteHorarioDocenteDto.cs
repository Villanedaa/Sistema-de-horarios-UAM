namespace SistemaHorarios.Modelos.DTOs.Reportes;

// Fila del reporte de horario asignado a un docente.
public class ReporteHorarioDocenteDto
{
    public int IdHorario { get; set; }
    public int IdDocente { get; set; }
    public string Docente { get; set; } = string.Empty;
    public string CodigoGrupo { get; set; } = string.Empty;
    public string NombreGrupo { get; set; } = string.Empty;
    public string Jornada { get; set; } = string.Empty;
    public int NumeroSemestre { get; set; }
    public string CodigoMateria { get; set; } = string.Empty;
    public string Materia { get; set; } = string.Empty;
    public string DiaSemana { get; set; } = string.Empty;
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFin { get; set; } = string.Empty;
    public string HorarioTexto => $"{HoraInicio} - {HoraFin}";
    public string Estado { get; set; } = string.Empty;
}
