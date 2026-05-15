using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.Entidades;

/// <summary>
/// Representa una franja horaria institucional.
///
/// Las franjas horarias son utilizadas para:
/// - horarios,
/// - disponibilidad docente,
/// - asignación académica,
/// - control de cruces.
/// </summary>
public class FranjaHoraria
{
    /// <summary>
    /// Identificador único de la franja horaria.
    /// </summary>
    [Key]
    public int IdFranjaHoraria { get; set; }

    /// <summary>
    /// Día de la semana.
    /// </summary>
    public string DiaSemana { get; set; } =
        string.Empty;

    /// <summary>
    /// Hora de inicio de la franja.
    /// </summary>
    public TimeSpan HoraInicio { get; set; }

    /// <summary>
    /// Hora de finalización de la franja.
    /// </summary>
    public TimeSpan HoraFin { get; set; }

    /// <summary>
    /// Indica si la franja está activa.
    /// </summary>
    public bool Activa { get; set; } = true;
}