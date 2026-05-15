using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SistemaHorarios.Modelos.Entidades
{
    public class SemestrePlan
    {
        [Key]
        public int IdSemestrePlan { get; set; }

        public int IdPlanAcademico { get; set; }

        public int NumeroSemestre { get; set; }

        [ForeignKey(nameof(IdPlanAcademico))]
        public PlanAcademico? PlanAcademico { get; set; }
    }
}
