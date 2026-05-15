using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.FranjasHorarias;

public class CrearFranjaHorariaDto
{
    [Required]
    public string DiaSemana { get; set; } =
        string.Empty;

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }
}