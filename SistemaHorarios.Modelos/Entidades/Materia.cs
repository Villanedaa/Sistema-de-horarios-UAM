namespace SistemaHorarios.Modelos.Entidades;

public class Materia
{
    public int IdMateria { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public int Creditos { get; set; }

    public int IntensidadHorariaSemanal { get; set; }

    public int SemestreSugerido { get; set; }

    public bool Activa { get; set; } = true;

    public Materia()
    {
        IdMateria = 0;
        Codigo = string.Empty;
        Nombre = string.Empty;
        Creditos = 0;
        IntensidadHorariaSemanal = 0;
        SemestreSugerido = 0;
        Activa = true;
    }
}