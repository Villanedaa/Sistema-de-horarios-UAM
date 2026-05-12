using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaHorarios.Modelos.Entidades;

public class Rol
{
    public int IdRol { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}