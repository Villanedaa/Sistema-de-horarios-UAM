using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaHorarios.Modelos.Entidades;

public class Horario
{
    public int IdHorario { get; set; }
    public int IdGrupo { get; set; }
    public int IdMateria { get; set; }
    public int IdDocente { get; set; }
    public string Dia { get; set; } = string.Empty;
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFinal { get; set; }
    public string Jornada { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Observacion { get; set; } = string.Empty;
}