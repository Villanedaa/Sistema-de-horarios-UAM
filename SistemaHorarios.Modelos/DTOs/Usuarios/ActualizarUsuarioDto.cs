using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.Usuarios;

using System.ComponentModel.DataAnnotations;



public class ActualizarUsuarioDto
{
    [Required]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    public string Cedula { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CorreoInstitucional { get; set; } = string.Empty;

    [Required]
    public int IdRol { get; set; }

    [Required]
    public string Estado { get; set; } = "Activo";

    public string Celular { get; set; } = string.Empty;
}