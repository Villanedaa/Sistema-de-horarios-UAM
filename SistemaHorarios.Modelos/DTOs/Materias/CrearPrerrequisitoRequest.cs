using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHorarios.Modelos.DTOs.Materias
{
    // Representa la información necesaria para asignar un prerrequisito a una materia.
    public class CrearPrerrequisitoRequest
    {
        public int IdMateria { get; set; }

        public int IdMateriaPrerrequisito { get; set; }
    }
}