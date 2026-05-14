using System.ComponentModel.DataAnnotations;

namespace SistemaHorarios.Modelos.Entidades;

public class Materia

    
{
    [Key]
    public int IdMateria { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int Creditos { get; set; }
    public int IntensidadHorariaSemanal { get; set; }
    public int Semestre { get; set; }
    public bool Activa { get; set; } = true;
}