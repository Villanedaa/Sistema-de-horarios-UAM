namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa un conflicto detectado al generar o asignar un horario.
public class ConflictoHorarioResponse
{
    public string TipoConflicto { get; set; } = string.Empty;

    public string Mensaje { get; set; } = string.Empty;

    public int? IdHorarioRelacionado { get; set; }

    public string Detalle { get; set; } = string.Empty;
}