using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.PlanAcademico;

public class CrearPlanAcademicoDto
{
    [Required(ErrorMessage = "El nombre del plan académico es obligatorio.")]
    [MinLength(3, ErrorMessage = "El nombre debe tener mínimo 3 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El programa es obligatorio.")]
    [MinLength(3, ErrorMessage = "El programa debe tener mínimo 3 caracteres.")]
    public string Programa { get; set; } = string.Empty;

    [Range(2000, 2100, ErrorMessage = "El año debe estar entre 2000 y 2100.")]
    public int Anio { get; set; }

    public string Estado { get; set; } = "Activo";
}