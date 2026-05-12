using System;
using System.Collections.Generic;
using System.Text;

namespace SistemaHorarios.Modelos.Entidades;

public class Grupo
{
    public int IdGrupo { get; set; }
    public string NombreGrupo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Jornada { get; set; } = string.Empty;
    public int IdMateria { get; set; }
    public int CupoMaximo { get; set; }
    public int CuposOcupados { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}