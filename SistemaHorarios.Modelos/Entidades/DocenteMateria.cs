using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHorarios.Modelos.Entidades;

// Relaciona un docente con una materia que puede dictar.
public class DocenteMateria
{
    [Key]
    public int IdDocenteMateria { get; set; }

    public int IdDocente { get; set; }

    public int IdMateria { get; set; }

    public bool Activo { get; set; } = true;

    [ForeignKey(nameof(IdDocente))]
    public Docente? Docente { get; set; }

    [ForeignKey(nameof(IdMateria))]
    public Materia? Materia { get; set; }
}