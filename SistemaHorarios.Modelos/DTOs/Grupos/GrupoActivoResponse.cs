namespace SistemaHorarios.Modelos.DTOs.Grupos;

// Representa un grupo activo para listas de selección.
public class GrupoActivoResponse
{
    public int IdGrupo { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public int NumeroSemestre { get; set; }

    public int IdPlanAcademico { get; set; }

    public string Jornada { get; set; } = string.Empty;
}
