using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Logica.Excepciones;
using SistemaHorarios.Modelos.DTOs.Docentes;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Docentes;

public class GestorDocente : IGestorDocente
{
    private readonly IDocenteRepository _repository;

    public GestorDocente(IDocenteRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DocenteResponse>> ObtenerTodosAsync()
    {
        var docentes = await _repository.ObtenerTodosAsync();
        var responses = new List<DocenteResponse>();

        foreach (var d in docentes)
        {
            var materias = await _repository.ObtenerNombresMateriasAsync(d.IdDocente);
            responses.Add(new DocenteResponse
            {
                IdDocente = d.IdDocente,
                NombreCompleto = d.NombreCompleto,
                Identificacion = d.Identificacion,
                CorreoInstitucional = d.CorreoInstitucional,
                Activo = d.Activo,
                Materias = materias
            });
        }

        return responses;
    }

    public async Task<DocenteResponse?> ObtenerPorIdAsync(int id)
    {
        var docente = await _repository.ObtenerPorIdAsync(id);
        if (docente == null)
            return null;

        var materias = await _repository.ObtenerNombresMateriasAsync(id);

        return new DocenteResponse
        {
            IdDocente = docente.IdDocente,
            NombreCompleto = docente.NombreCompleto,
            Identificacion = docente.Identificacion,
            CorreoInstitucional = docente.CorreoInstitucional,
            Activo = docente.Activo,
            Materias = materias
        };
    }

    public async Task<DocenteResponse> CrearAsync(DocenteRequest dto)
    {
        var docente = new Docente
        {
            NombreCompleto = dto.NombreCompleto,
            Identificacion = dto.Identificacion,
            CorreoInstitucional = dto.CorreoInstitucional,
            Activo = dto.Activo
        };

        var creado = await _repository.CrearAsync(docente);

        var idsValidos = dto.IdsMateria.Where(id => id > 0).ToList();
        if (idsValidos.Count > 0)
            await _repository.ActualizarMateriasAsync(creado.IdDocente, idsValidos);

        var materias = await _repository.ObtenerNombresMateriasAsync(creado.IdDocente);

        return new DocenteResponse
        {
            IdDocente = creado.IdDocente,
            NombreCompleto = creado.NombreCompleto,
            Identificacion = creado.Identificacion,
            CorreoInstitucional = creado.CorreoInstitucional,
            Activo = creado.Activo,
            Materias = materias
        };
    }

    public async Task ActualizarAsync(int id, DocenteRequest dto)
    {
        var docente = await _repository.ObtenerPorIdAsync(id);
        if (docente == null)
            throw new NotFoundException("Docente no encontrado");

        docente.NombreCompleto = dto.NombreCompleto;
        docente.Identificacion = dto.Identificacion;
        docente.CorreoInstitucional = dto.CorreoInstitucional;
        docente.Activo = dto.Activo;

        await _repository.ActualizarAsync(docente);

        var idsValidos = dto.IdsMateria.Where(id => id > 0).ToList();
        await _repository.ActualizarMateriasAsync(id, idsValidos);
    }

    public async Task EliminarAsync(int id)
    {
        var docente = await _repository.ObtenerPorIdAsync(id);
        if (docente == null)
            throw new NotFoundException("Docente no encontrado");

        await _repository.EliminarAsync(docente);
    }
}
