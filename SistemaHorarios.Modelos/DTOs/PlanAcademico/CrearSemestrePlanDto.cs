using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.PlanAcademico;

public class CrearSemestrePlanDto
{
    [Range(1, 12, ErrorMessage = "El número de semestre debe estar entre 1 y 12.")]
    public int NumeroSemestre { get; set; }
}