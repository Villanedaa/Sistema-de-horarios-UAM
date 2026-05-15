using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.Entidades
{
    public class PlanAcademico
    {
        [Key]
        public int IdPlanAcademico { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Jornada { get; set; } = string.Empty;

        public int CantidadSemestres { get; set; }

        public bool Activo { get; set; } = true;
    }
}