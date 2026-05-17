namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa el resultado de bloquear una asignación de horario.
public class BloquearHorarioResponse
{
    public bool Bloqueado { get; set; }

    public string Motivo { get; set; } = string.Empty;

    public List<ConflictoHorarioResponse> Conflictos { get; set; } =
        new List<ConflictoHorarioResponse>();
}