using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHorarios.Modelos.DTOs.Auth;

public class RegistroUsuarioDto
{
    public string NombreCompleto { get; set; } = string.Empty;

    public string Cedula { get; set; } = string.Empty;

    public string CorreoInstitucional { get; set; } = string.Empty;

    public string Contrasena { get; set; } = string.Empty;

    public int IdRol { get; set; }
}