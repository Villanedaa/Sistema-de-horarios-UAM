namespace SistemaHorarios.Modelos.Entidades;

public class PlanAcademico
{
    public int IdPlanAcademico { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Programa { get; set; } = string.Empty;

    public int Anio { get; set; }

    public string Estado { get; set; } = "Activo";

    public ICollection<SemestrePlan> Semestres { get; set; } =
        new List<SemestrePlan>();
}