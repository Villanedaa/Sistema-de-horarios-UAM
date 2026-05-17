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

        return docentes.Select(x => new DocenteResponse
        {
            IdDocente = x.IdDocente,
            NombreCompleto = x.NombreCompleto,
            Identificacion = x.Identificacion,
            CorreoInstitucional = x.CorreoInstitucional,
            Activo = x.Activo
        }).ToList();
    }

    public async Task<DocenteResponse?> ObtenerPorIdAsync(int id)
    {
        var docente = await _repository.ObtenerPorIdAsync(id);

        if (docente == null)
            return null;

        return new DocenteResponse
        {
            IdDocente = docente.IdDocente,
            NombreCompleto = docente.NombreCompleto,
            Identificacion = docente.Identificacion,
            CorreoInstitucional = docente.CorreoInstitucional,
            Activo = docente.Activo
        };
    }

    public async Task<DocenteResponse> CrearAsync(DocenteRequest dto)
    {
        var docente = new Docente
        {
            NombreCompleto = dto.NombreCompleto,
            Identificacion = dto.Identificacion,
            CorreoInstitucional = dto.CorreoInstitucional,
            Activo = true
        };

        var creado = await _repository.CrearAsync(docente);

        return new DocenteResponse
        {
            IdDocente = creado.IdDocente,
            NombreCompleto = creado.NombreCompleto,
            Identificacion = creado.Identificacion,
            CorreoInstitucional = creado.CorreoInstitucional,
            Activo = creado.Activo
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

        await _repository.ActualizarAsync(docente);
    }

    public async Task EliminarAsync(int id)
    {
        var docente = await _repository.ObtenerPorIdAsync(id);

        if (docente == null)
            throw new NotFoundException("Docente no encontrado");

        await _repository.EliminarAsync(docente);
    }
}