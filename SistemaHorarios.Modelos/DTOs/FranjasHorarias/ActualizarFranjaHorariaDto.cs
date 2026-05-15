using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.FranjasHorarias;

public class ActualizarFranjaHorariaDto
{
    [Required]
    public string DiaSemana { get; set; } =
        string.Empty;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public bool Activa { get; set; }
}