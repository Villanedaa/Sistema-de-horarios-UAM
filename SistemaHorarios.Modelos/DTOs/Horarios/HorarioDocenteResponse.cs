namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa una asignación dentro del horario de un docente.
public class HorarioDocenteResponse
{
    public int IdHorario { get; set; }

    public int IdDocente { get; set; }

    public string NombreDocente { get; set; } = string.Empty;

    public string IdentificacionDocente { get; set; } = string.Empty;

    public string CorreoInstitucional { get; set; } = string.Empty;

    public int IdGrupo { get; set; }

    public string CodigoGrupo { get; set; } = string.Empty;

    public string NombreGrupo { get; set; } = string.Empty;

    public int IdMateria { get; set; }

    public string CodigoMateria { get; set; } = string.Empty;

    public string NombreMateria { get; set; } = string.Empty;

    public string DiaSemana { get; set; } = string.Empty;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public string HorarioTexto { get; set; } = string.Empty;

    public string EstadoTexto { get; set; } = string.Empty;
}