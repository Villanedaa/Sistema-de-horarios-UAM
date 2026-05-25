using SistemaHorarios.Modelos.DTOs.Docentes;

namespace SistemaHorarios.Logica.Negocio.Docentes;

public interface IGestorDocente
{
    Task<List<DocenteResponse>> ObtenerTodosAsync();

    Task<DocenteResponse?> ObtenerPorIdAsync(int id);

    Task<DocenteResponse> CrearAsync(DocenteRequest dto);

    Task ActualizarAsync(int id, DocenteRequest dto);

    Task EliminarAsync(int id);
 
    Task ActivarAsync(int id);
}