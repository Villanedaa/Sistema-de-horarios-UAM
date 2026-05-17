using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.Roles;

public class CrearRolDto
{
    [Required]
    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;
}