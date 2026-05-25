namespace SistemaHorarios.Modelos.DTOs.Reportes;

// Fila del reporte oficial de horario por grupo académico.
public class ReporteHorarioGrupoDto
{
    public int IdHorario { get; set; }
    public int IdGrupo { get; set; }
    public string CodigoGrupo { get; set; } = string.Empty;
    public string NombreGrupo { get; set; } = string.Empty;
    public string Jornada { get; set; } = string.Empty;
    public int NumeroSemestre { get; set; }
    public string CodigoMateria { get; set; } = string.Empty;
    public string Materia { get; set; } = string.Empty;
    public string Docente { get; set; } = string.Empty;
    public string DiaSemana { get; set; } = string.Empty;
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFin { get; set; } = string.Empty;
    public string HorarioTexto => $"{HoraInicio} - {HoraFin}";
    public string Estado { get; set; } = string.Empty;
    public string Observacion { get; set; } = string.Empty;
}
