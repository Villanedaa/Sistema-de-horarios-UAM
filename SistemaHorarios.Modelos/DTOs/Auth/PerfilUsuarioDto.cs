using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHorarios.Modelos.DTOs.Auth;

/// <summary>
/// DTO utilizado para solicitudes 
/// de perfil de usuario.
/// </summary>
public class PerfilUsuarioDto
{
    public int IdUsuario { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string CorreoInstitucional { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;
}