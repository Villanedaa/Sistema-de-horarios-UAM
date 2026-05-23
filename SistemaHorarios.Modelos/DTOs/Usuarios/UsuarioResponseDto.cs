namespace SistemaHorarios.Modelos.DTOs.Usuarios;

public class UsuarioResponseDto
{
    public int IdUsuario { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Cedula { get; set; } = string.Empty;

    public string CorreoInstitucional { get; set; } = string.Empty;

    public int IdRol { get; set; }

    public string Rol { get; set; } = string.Empty;

    public string Estado { get; set; } = string.Empty;

    public string Celular { get; set; } = string.Empty;

    public string? FotoPerfilUrl { get; set; }
}