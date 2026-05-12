using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaHorarios.Modelos.Entidades;

public class Docente
{
    public int IdDocente { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string CorreoInstitucional { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}