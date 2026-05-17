namespace SistemaHorarios.Modelos.DTOs.Docentes;

public class ActualizarDisponibilidadDocenteRequest
{
    public List<DisponibilidadDocenteRequest> Disponibilidades { get; set; }
        = new();
}