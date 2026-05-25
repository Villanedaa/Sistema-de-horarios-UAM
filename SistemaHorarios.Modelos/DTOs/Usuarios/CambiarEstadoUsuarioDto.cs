using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.Usuarios;

/// <summary>
/// DTO para activar o inactivar usuarios del sistema.
/// </summary>
public class CambiarEstadoUsuarioDto
{
    [Required]
    public string Estado { get; set; } = "Activo";
}
