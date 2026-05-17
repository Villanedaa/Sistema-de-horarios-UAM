using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHorarios.Modelos.Entidades;

/// <summary>
/// Representa un usuario del sistema de horarios.
///
/// Esta entidad es utilizada para:
/// - autenticación,
/// - autorización,
/// - control de acceso,
/// - generación de JWT,
/// - asignación de roles.
/// </summary>
public class Usuario
{
    /// <summary>
    /// Identificador único del usuario.
    /// Clave primaria de la tabla Usuarios.
    /// </summary>
    [Key]
    public int IdUsuario { get; set; }

    /// <summary>
    /// Nombre completo del usuario.
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Documento de identificación del usuario.
    /// </summary>
    public string Cedula { get; set; } = string.Empty;

    /// <summary>
    /// Correo institucional utilizado para autenticación.
    /// </summary>
    public string CorreoInstitucional { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña almacenada de forma segura mediante BCrypt.
    /// Nunca se guarda texto plano.
    /// </summary>
    public string ContrasenaHash { get; set; } = string.Empty;

    /// <summary>
    /// Llave foránea que referencia el rol del usuario.
    /// </summary>
    public int IdRol { get; set; }

    /// <summary>
    /// Propiedad de navegación hacia el rol asociado.
    /// Relación muchos a uno:
    /// muchos usuarios pueden pertenecer a un mismo rol.
    /// </summary>
    [ForeignKey(nameof(IdRol))]
    public Rol Rol { get; set; } = null!;

    /// <summary>
    /// Estado actual del usuario.
    /// Ejemplos:
    /// Activo,
    /// Inactivo,
    /// Suspendido.
    /// </summary>
    public string Estado { get; set; } = "Activo";
}