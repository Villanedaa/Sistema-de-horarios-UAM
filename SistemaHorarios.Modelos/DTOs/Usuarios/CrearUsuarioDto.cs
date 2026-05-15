using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.Usuarios;

public class CrearUsuarioDto
{
    [Required]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    public string Cedula { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CorreoInstitucional { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Contrasena { get; set; } = string.Empty;

    [Required]
    public int IdRol { get; set; }

    public string Estado { get; set; } = "Activo";
}