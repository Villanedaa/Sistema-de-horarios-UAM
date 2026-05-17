namespace SistemaHorarios.Modelos.DTOs.Docentes;

public class DocenteRequest
{
    public string NombreCompleto { get; set; } = string.Empty;

    public string Identificacion { get; set; } = string.Empty;

    public string CorreoInstitucional { get; set; } = string.Empty;
}