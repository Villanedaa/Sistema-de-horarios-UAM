using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaHorarios.Modelos.Entidades;

// Representa una asignación de clase dentro de un horario académico.
public class Horario
{
    [Key]
    public int IdHorario { get; set; }

    public int IdGrupo { get; set; }

    public int IdMateria { get; set; }

    public int IdDocente { get; set; }

    public int IdFranjaHoraria { get; set; }

    public string Observacion { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    [ForeignKey(nameof(IdGrupo))]
    public Grupo? Grupo { get; set; }

    [ForeignKey(nameof(IdMateria))]
    public Materia? Materia { get; set; }

    [ForeignKey(nameof(IdDocente))]
    public Docente? Docente { get; set; }

    [ForeignKey(nameof(IdFranjaHoraria))]
    public FranjaHoraria? FranjaHoraria { get; set; }
}