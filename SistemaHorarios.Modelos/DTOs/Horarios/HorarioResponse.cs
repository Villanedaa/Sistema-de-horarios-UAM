namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa toda la información de una asignación de horario académico.
public class HorarioResponse
{
    public int IdHorario { get; set; }

    public int IdGrupo { get; set; }

    public string CodigoGrupo { get; set; } = string.Empty;

    public string NombreGrupo { get; set; } = string.Empty;

    public string Jornada { get; set; } = string.Empty;

    public string TipoGrupo { get; set; } = string.Empty;

    public int IdMateria { get; set; }

    public string CodigoMateria { get; set; } = string.Empty;

    public string NombreMateria { get; set; } = string.Empty;

    public int IdDocente { get; set; }

    public string NombreDocente { get; set; } = string.Empty;

    public string IdentificacionDocente { get; set; } = string.Empty;

    public int IdFranjaHoraria { get; set; }

    public string DiaSemana { get; set; } = string.Empty;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public string HorarioTexto { get; set; } = string.Empty;

    public string Observacion { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public string EstadoTexto { get; set; } = string.Empty;

    public string MotivoRechazo { get; set; } = string.Empty;
}
