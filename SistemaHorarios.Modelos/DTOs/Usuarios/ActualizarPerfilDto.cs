using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.Usuarios;

public class ActualizarPerfilDto
{
    [Required]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string CorreoInstitucional { get; set; } = string.Empty;
}