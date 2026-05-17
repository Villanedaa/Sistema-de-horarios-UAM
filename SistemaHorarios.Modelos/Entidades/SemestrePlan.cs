namespace SistemaHorarios.Modelos.Entidades;

public class SemestrePlan
{
    public int IdSemestrePlan { get; set; }

    public int NumeroSemestre { get; set; }

    public int IdPlanAcademico { get; set; }

    public PlanAcademico? PlanAcademico { get; set; }

    public ICollection<MateriaPlan> MateriasPlan { get; set; } =
        new List<MateriaPlan>();
}