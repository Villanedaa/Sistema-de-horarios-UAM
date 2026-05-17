namespace SistemaHorarios.Modelos.DTOs.Docentes;

public class DisponibilidadDocenteRequest
{
    public string Dia { get; set; } = string.Empty;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public bool Disponible { get; set; } = true;
}