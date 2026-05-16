namespace SistemaHorarios.Modelos.Entidades;

public class MateriaPlan
{
    public int IdMateriaPlan { get; set; }

    public int IdSemestrePlan { get; set; }

    public SemestrePlan? SemestrePlan { get; set; }

    public int IdMateria { get; set; }

    public Materia? Materia { get; set; }
}