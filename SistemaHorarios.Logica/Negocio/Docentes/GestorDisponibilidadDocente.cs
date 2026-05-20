using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SistemaHorarios.Datos.Interfaces;

using SistemaHorarios.Logica.Excepciones;

using SistemaHorarios.Modelos.DTOs.Docentes;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Docentes;

public class GestorDisponibilidadDocente
    : IGestorDisponibilidadDocente
{
    private readonly IDisponibilidadDocenteRepository
        _repository;

    private readonly IDocenteRepository
        _docenteRepository;

    public GestorDisponibilidadDocente(
        IDisponibilidadDocenteRepository repository,

        IDocenteRepository docenteRepository)
    {
        _repository = repository;

        _docenteRepository = docenteRepository;
    }

    public async Task<List<DisponibilidadDocenteResponse>>
        ObtenerDisponibilidadAsync(int idDocente)
    {
        var existeDocente = await _docenteRepository
            .ExistePorIdAsync(idDocente);

        if (!existeDocente)
            throw new NotFoundException(
                "Docente no encontrado");

        var disponibilidades = await _repository
            .ObtenerPorDocenteAsync(idDocente);

        return disponibilidades.Select(x =>
            new DisponibilidadDocenteResponse
            {
                IdDisponibilidadDocente =
                    x.IdDisponibilidadDocente,

                IdDocente = x.IdDocente,

                Dia = x.Dia,

                HoraInicio = x.HoraInicio,

                HoraFin = x.HoraFin,

                Disponible = x.Disponible
            }).ToList();
    }

    public async Task ActualizarDisponibilidadAsync(
        int idDocente,

        ActualizarDisponibilidadDocenteRequest request)
    {
        var existeDocente = await _docenteRepository
            .ExistePorIdAsync(idDocente);

        if (!existeDocente)
            throw new NotFoundException(
                "Docente no encontrado");

        if (request?.Disponibilidades == null)
            throw new InvalidOperationException(
                "Debe enviar la lista de disponibilidades");

        foreach (var disponibilidad in request.Disponibilidades)
        {
            if (string.IsNullOrWhiteSpace(disponibilidad.Dia))
                throw new InvalidOperationException(
                    "El día de la disponibilidad es obligatorio");

            if (disponibilidad.HoraFin <= disponibilidad.HoraInicio)
                throw new InvalidOperationException(
                    "La hora fin de la disponibilidad debe ser mayor que la hora inicio");
        }

        await _repository
            .EliminarPorDocenteAsync(idDocente);

        var disponibilidades = request.Disponibilidades
            .Select(x => new DisponibilidadDocente
            {
                IdDocente = idDocente,

                Dia = x.Dia.Trim(),

                HoraInicio = x.HoraInicio,

                HoraFin = x.HoraFin,

                Disponible = x.Disponible
            }).ToList();

        await _repository
            .CrearRangoAsync(disponibilidades);
    }
}

