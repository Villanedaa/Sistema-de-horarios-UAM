using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.DTOs.PlanAcademico;

public class AgregarMateriaPlanDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una materia válida.")]
    public int IdMateria { get; set; }
}
