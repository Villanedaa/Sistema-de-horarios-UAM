using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.Auth;

/// <summary>
/// DTO utilizado para solicitudes
/// de registro de usuario.
/// </summary>
public class RegistroUsuarioDto
{
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    [MinLength(3, ErrorMessage = "El nombre completo debe tener mínimo 3 caracteres.")]
    public string NombreCompleto
    {
        get; set;
    } = string.Empty;

    [Required(ErrorMessage = "La cédula es obligatoria.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "La cédula solo debe contener números.")]
    public string Cedula
    {
        get; set;
    } = string.Empty;

    [Required(ErrorMessage = "El correo institucional es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo institucional no tiene un formato válido.")]
    public string CorreoInstitucional
    {
        get; set;
    } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener mínimo 6 caracteres.")]
    public string Contrasena
    {
        get; set;
    } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "El rol seleccionado no es válido.")]
    public int IdRol
    {
        get; set;
    }
}