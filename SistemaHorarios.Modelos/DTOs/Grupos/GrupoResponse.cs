namespace SistemaHorarios.Modelos.DTOs.Grupos;

// Representa toda la información de un grupo académico.
public class GrupoResponse
{
    public int IdGrupo { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Jornada { get; set; } = string.Empty;

    public string TipoGrupo { get; set; } = string.Empty;

    public int NumeroSemestre { get; set; }

    public int CantidadEstudiantes { get; set; }

    public int IdPlanAcademico { get; set; }

    public bool Activo { get; set; }

    public string EstadoTexto { get; set; } = string.Empty;
}