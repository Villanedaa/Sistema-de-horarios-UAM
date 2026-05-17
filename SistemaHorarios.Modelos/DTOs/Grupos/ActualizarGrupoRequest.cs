namespace SistemaHorarios.Modelos.DTOs.Grupos;

// Representa los datos necesarios para actualizar un grupo académico.
public class ActualizarGrupoRequest
{
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Jornada { get; set; } = string.Empty;

    public string TipoGrupo { get; set; } = string.Empty;

    public int NumeroSemestre { get; set; }

    public int CantidadEstudiantes { get; set; }

    public int IdPlanAcademico { get; set; }

    public bool Activo { get; set; }
}