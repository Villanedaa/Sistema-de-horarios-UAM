namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa los datos necesarios para actualizar una asignación de horario.
public class ActualizarHorarioRequest
{
    public int IdGrupo { get; set; }

    public int IdMateria { get; set; }

    public int IdDocente { get; set; }

    public int IdFranjaHoraria { get; set; }

    public string Observacion { get; set; } = string.Empty;

    public bool Activo { get; set; }
}