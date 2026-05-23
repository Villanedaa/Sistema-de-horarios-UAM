using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Logica.Excepciones;
using SistemaHorarios.Modelos.DTOs.Docentes;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Docentes;

public class GestorDisponibilidadDocente : IGestorDisponibilidadDocente
{
    private readonly IDisponibilidadDocenteRepository _repository;
    private readonly IDocenteRepository _docenteRepository;

    public GestorDisponibilidadDocente(
        IDisponibilidadDocenteRepository repository,
        IDocenteRepository docenteRepository)
    {
        _repository = repository;
        _docenteRepository = docenteRepository;
    }

    public async Task<List<DisponibilidadDocenteResponse>> ObtenerDisponibilidadAsync(
        int idDocente)
    {
        bool existeDocente =
            await _docenteRepository.ExistePorIdAsync(idDocente);

        if (!existeDocente)
        {
            throw new NotFoundException("Docente no encontrado");
        }

        List<DisponibilidadDocente> disponibilidades =
            await _repository.ObtenerPorDocenteAsync(idDocente);

        return disponibilidades
            .Where(x => x.HoraInicio != TimeSpan.Zero &&
                        x.HoraFin != TimeSpan.Zero &&
                        x.Disponible)
            .GroupBy(x => new
            {
                x.IdDocente,
                x.Dia,
                x.HoraInicio,
                x.HoraFin
            })
            .Select(g => g.First())
            .OrderBy(x => x.Dia)
            .ThenBy(x => x.HoraInicio)
            .Select(x => new DisponibilidadDocenteResponse
            {
                IdDisponibilidadDocente = x.IdDisponibilidadDocente,
                IdDocente = x.IdDocente,
                Dia = x.Dia,
                HoraInicio = x.HoraInicio,
                HoraFin = x.HoraFin,
                Disponible = x.Disponible
            })
            .ToList();
    }

    public async Task ActualizarDisponibilidadAsync(
        int idDocente,
        ActualizarDisponibilidadDocenteRequest request)
    {
        bool existeDocente =
            await _docenteRepository.ExistePorIdAsync(idDocente);

        if (!existeDocente)
        {
            throw new NotFoundException("Docente no encontrado");
        }

        await _repository.EliminarPorDocenteAsync(idDocente);

        List<DisponibilidadDocente> disponibilidades =
            request.Disponibilidades
                .Where(x => x.Disponible &&
                            x.HoraInicio != TimeSpan.Zero &&
                            x.HoraFin != TimeSpan.Zero &&
                            x.HoraFin > x.HoraInicio &&
                            !string.IsNullOrWhiteSpace(x.Dia))
                .GroupBy(x => new
                {
                    Dia = x.Dia.Trim(),
                    x.HoraInicio,
                    x.HoraFin
                })
                .Select(g => g.First())
                .Select(x => new DisponibilidadDocente
                {
                    IdDocente = idDocente,
                    Dia = x.Dia.Trim(),
                    HoraInicio = x.HoraInicio,
                    HoraFin = x.HoraFin,
                    Disponible = true
                })
                .ToList();

        if (disponibilidades.Count == 0)
        {
            return;
        }

        await _repository.CrearRangoAsync(disponibilidades);
    }
}