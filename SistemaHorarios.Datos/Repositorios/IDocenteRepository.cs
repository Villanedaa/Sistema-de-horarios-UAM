using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Interfaces;

public interface IDocenteRepository
{
    Task<List<Docente>> ObtenerTodosAsync();

    Task<Docente?> ObtenerPorIdAsync(int id);

    Task<Docente> CrearAsync(Docente docente);

    Task ActualizarAsync(Docente docente);

    Task EliminarAsync(Docente docente);

    Task ActivarAsync(Docente docente);

    Task<bool> ExistePorIdAsync(int id);

    Task ActualizarMateriasAsync(int idDocente, List<int> idsMateria);

    Task<List<string>> ObtenerNombresMateriasAsync(int idDocente);
}