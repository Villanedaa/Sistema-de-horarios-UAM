using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHorarios.Modelos.Entidades;

// Representa un grupo académico asociado a un plan académico y a una jornada.
public class Grupo
{
    [Key]
    public int IdGrupo { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Jornada { get; set; } = string.Empty;

    public string TipoGrupo { get; set; } = string.Empty;

    public int NumeroSemestre { get; set; }

    public int CantidadEstudiantes { get; set; }

    public int IdPlanAcademico { get; set; }

    public bool Activo { get; set; } = true;

    [ForeignKey(nameof(IdPlanAcademico))]
    public PlanAcademico? PlanAcademico { get; set; }
}