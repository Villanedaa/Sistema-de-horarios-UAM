using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.Auth;

public class LoginRequestDto
{
    [Required(
        ErrorMessage =
            "El correo es obligatorio")]
    [EmailAddress(
        ErrorMessage =
            "Correo inválido")]
    public string CorreoInstitucional
    {
        get; set;
    } = string.Empty;

    [Required(
        ErrorMessage =
            "La contraseña es obligatoria")]
    [MinLength(
        6,
        ErrorMessage =
            "La contraseña debe tener mínimo 6 caracteres")]
    public string Contrasena
    {
        get; set;
    } = string.Empty;
}