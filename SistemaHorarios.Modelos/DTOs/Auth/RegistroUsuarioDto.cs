using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.Auth;

public class RegistroUsuarioDto
{
    [Required]
    public string NombreCompleto
    {
        get; set;
    } = string.Empty;

    [Required]
    public string Cedula
    {
        get; set;
    } = string.Empty;

    [Required]
    [EmailAddress]
    public string CorreoInstitucional
    {
        get; set;
    } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Contrasena
    {
        get; set;
    } = string.Empty;

    [Range(1, int.MaxValue)]
    public int IdRol
    {
        get; set;
    }
}