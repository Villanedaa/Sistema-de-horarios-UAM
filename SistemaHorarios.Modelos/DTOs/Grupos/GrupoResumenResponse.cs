namespace SistemaHorarios.Modelos.DTOs.Grupos;

// Representa un grupo académico en formato resumido para listados.
public class GrupoResumenResponse
{
    public int IdGrupo { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Jornada { get; set; } = string.Empty;

    public string TipoGrupo { get; set; } = string.Empty;

    public int NumeroSemestre { get; set; }

    public int CantidadEstudiantes { get; set; }

    public int IdPlanAcademico { get; set; }

    public string Materia { get; set; } = string.Empty;

    public string Dias { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public string EstadoTexto { get; set; } = string.Empty;
}