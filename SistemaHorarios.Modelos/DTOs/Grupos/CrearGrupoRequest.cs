namespace SistemaHorarios.Modelos.DTOs.Grupos;

// Representa los datos necesarios para crear un grupo académico.
public class CrearGrupoRequest
{
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Jornada { get; set; } = string.Empty;

    public string TipoGrupo { get; set; } = string.Empty;

    public int NumeroSemestre { get; set; }

    public int CantidadEstudiantes { get; set; }

    public int IdPlanAcademico { get; set; }

    public string Materia { get; set; } = string.Empty;

    public string Dias { get; set; } = string.Empty;
}