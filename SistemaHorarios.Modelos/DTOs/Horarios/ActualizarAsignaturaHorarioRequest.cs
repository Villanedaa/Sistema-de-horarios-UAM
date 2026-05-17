namespace SistemaHorarios.Modelos.DTOs.Horarios;

// Representa los datos necesarios para cambiar la asignatura, docente o franja de un horario.
public class ActualizarAsignaturaHorarioRequest
{
    public int IdMateria { get; set; }

    public int IdDocente { get; set; }

    public int IdFranjaHoraria { get; set; }

    public string Observacion { get; set; } = string.Empty;
}