using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaHorarios.Modelos.Entidades;

public class Prerrequisito
{
    public int IdPrerrequisito { get; set; }
    public int IdMateria { get; set; }
    public int IdMateriaPrerrequisito { get; set; }
    public bool Activo { get; set; } = true;
}
