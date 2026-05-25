namespace SistemaHorarios.Modelos.DTOs.Docentes;

public class DocenteResponse
{
    public int IdDocente { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Identificacion { get; set; } = string.Empty;

    public string CorreoInstitucional { get; set; } = string.Empty;

    public bool Activo { get; set; }

    public List<string> Materias { get; set; } = new();
}