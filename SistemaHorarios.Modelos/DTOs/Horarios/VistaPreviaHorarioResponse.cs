using SistemaHorarios.Modelos.DTOs.Horarios;

namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa la vista previa de un horario antes de aprobarlo o guardarlo.
public class VistaPreviaHorarioResponse
{
    public bool GeneradoCorrectamente { get; set; }

    public string Mensaje { get; set; } = string.Empty;

    public List<HorarioResponse> Horarios { get; set; } =
        new List<HorarioResponse>();

    public List<ConflictoHorarioResponse> Conflictos { get; set; } =
        new List<ConflictoHorarioResponse>();
}