using System.ComponentModel.DataAnnotations;


namespace SistemaHorarios.Modelos.Entidades;

public class DisponibilidadDocente
{
    [Key]
    public int IdDisponibilidadDocente { get; set; }
    public int IdDocente { get; set; }
    public string Dia { get; set; } = string.Empty;
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public bool Disponible { get; set; } = true;
}