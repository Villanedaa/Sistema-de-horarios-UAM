using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHorarios.Modelos.Entidades;

// Representa una materia registrada dentro del sistema.
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