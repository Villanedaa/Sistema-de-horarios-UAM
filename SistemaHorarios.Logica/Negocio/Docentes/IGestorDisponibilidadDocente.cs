using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaHorarios.Modelos.DTOs.Docentes;

namespace SistemaHorarios.Logica.Negocio.Docentes;

public interface IGestorDisponibilidadDocente
{
    Task<List<DisponibilidadDocenteResponse>>
        ObtenerDisponibilidadAsync(int idDocente);

    Task ActualizarDisponibilidadAsync(
        int idDocente,
        ActualizarDisponibilidadDocenteRequest request);
}