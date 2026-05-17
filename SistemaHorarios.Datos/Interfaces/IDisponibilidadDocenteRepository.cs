using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Interfaces;

public interface IDisponibilidadDocenteRepository
{
    Task<List<DisponibilidadDocente>> ObtenerPorDocenteAsync(int idDocente);

    Task EliminarPorDocenteAsync(int idDocente);

    Task CrearRangoAsync(List<DisponibilidadDocente> disponibilidades);
}