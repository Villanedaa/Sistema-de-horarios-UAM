using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaHorarios.Modelos.Entidades;

public class DocenteMateria
{
    public int IdDocenteMateria { get; set; }
    public int IdDocente { get; set; }
    public int IdMateria { get; set; }
    public bool Activo { get; set; } = true;
}