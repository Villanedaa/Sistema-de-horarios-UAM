using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHorarios.Modelos.DTOs.Auth;
/// <summary>
/// DTO utilizado para respuestas de solicitudes
/// de inicio de sesión.
/// </summary>
public class LoginResponseDto
{
    public int IdUsuario { get; set; }

    public string CorreoInstitucional { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public string NombreCompleto { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;
}
