namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa la disponibilidad de un docente para una franja horaria.
public class DisponibilidadDocenteHorarioResponse
{
    public int IdDocente { get; set; }

    public string NombreDocente { get; set; } = string.Empty;

    public string IdentificacionDocente { get; set; } = string.Empty;

    public string DiaSemana { get; set; } = string.Empty;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public bool Disponible { get; set; }
}