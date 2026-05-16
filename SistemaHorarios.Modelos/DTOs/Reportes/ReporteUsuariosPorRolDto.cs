namespace SistemaHorarios.Modelos.DTOs.Reportes;

public class ReporteUsuariosPorRolDto
{
    public string Rol { get; set; } = string.Empty;

    public int TotalUsuarios { get; set; }
}