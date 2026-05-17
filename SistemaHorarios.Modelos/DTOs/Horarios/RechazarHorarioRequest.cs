namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa los datos necesarios para rechazar una propuesta de horario.
public class RechazarHorarioRequest
{
    public int IdHorario { get; set; }

    public string MotivoRechazo { get; set; } = string.Empty;
}