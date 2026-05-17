using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.Usuarios;

public class CambiarContrasenaDto
{
    [Required]
    public string ContrasenaActual { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NuevaContrasena { get; set; } = string.Empty;
}