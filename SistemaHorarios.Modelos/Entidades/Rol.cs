using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.Entidades;

/// <summary>
/// Representa un rol dentro del sistema.
///
/// Los roles controlan:
/// - permisos,
/// - autorización,
/// - acceso a módulos,
/// - policies del sistema.
/// </summary>
public class Rol
{
    /// <summary>
    /// Identificador único del rol.
    /// </summary>
    [Key]
    public int IdRol { get; set; }

    /// <summary>
    /// Nombre del rol.
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción funcional del rol.
    /// </summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Indica si el rol está activo.
    /// </summary>
    public bool Activo { get; set; } = true;
}