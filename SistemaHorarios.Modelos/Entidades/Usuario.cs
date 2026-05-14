using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHorarios.Modelos.Entidades;

public class Usuario
{
    [Key]
    public int IdUsuario { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Cedula { get; set; } = string.Empty;

    public string CorreoInstitucional { get; set; } = string.Empty;

    public string ContrasenaHash { get; set; } = string.Empty;

    public int IdRol { get; set; }

    [ForeignKey(nameof(IdRol))]
    public Rol Rol { get; set; } = null!;

    public string Estado { get; set; } = "Activo";
}
