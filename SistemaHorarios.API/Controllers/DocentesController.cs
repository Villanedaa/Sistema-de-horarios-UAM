using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Docentes;
using SistemaHorarios.Modelos.DTOs.Docentes;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador,Coordinador")]
public class DocentesController : ControllerBase
{
    private readonly IGestorDocente _gestor;
    private readonly IGestorDisponibilidadDocente _gestorDisponibilidad;

    public DocentesController(
        IGestorDocente gestor,
        IGestorDisponibilidadDocente gestorDisponibilidad)
    {
        _gestor = gestor;
        _gestorDisponibilidad = gestorDisponibilidad;
    }

    // =====================================================
    // CRUD DOCENTES
    // =====================================================

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var docentes = await _gestor.ObtenerTodosAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Docentes consultados correctamente.",
            Data = docentes
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var docente = await _gestor.ObtenerPorIdAsync(id);

        if (docente == null)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "El docente no existe.",
                Data = null
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Docente consultado correctamente.",
            Data = docente
        });
    }

    [HttpPost]
    public async Task<IActionResult> Crear(DocenteRequest request)
    {
        var docente = await _gestor.CrearAsync(request);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = docente.IdDocente },
            new ApiResponse<object>
            {
                Success = true,
                Message = "Docente creado correctamente.",
                Data = docente
            }
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        int id,
        DocenteRequest request)
    {
        await _gestor.ActualizarAsync(id, request);

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Docente actualizado correctamente.",
            Data = id
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
    	await _gestor.EliminarAsync(id);

    	return Ok(new ApiResponse<int>
    	{
            Success = true,
            Message = "Docente inactivado correctamente.",
            Data = id
        });
    }


    [HttpPatch("{id}/activar")]
    public async Task<IActionResult> Activar(int id)
    {
    	await _gestor.ActivarAsync(id);

    	return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Docente activado correctamente.",
            Data = id
        });
    }

    // =====================================================
    // DISPONIBILIDAD DOCENTE
    // =====================================================

    [HttpGet("{id}/disponibilidad")]
    public async Task<IActionResult> ObtenerDisponibilidad(int id)
    {
        var disponibilidad =
            await _gestorDisponibilidad.ObtenerDisponibilidadAsync(id);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Disponibilidad docente consultada correctamente.",
            Data = disponibilidad
        });
    }

    [HttpPut("{id}/disponibilidad")]
    public async Task<IActionResult> ActualizarDisponibilidad(
        int id,
        ActualizarDisponibilidadDocenteRequest request)
    {
        await _gestorDisponibilidad.ActualizarDisponibilidadAsync(id, request);

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Disponibilidad docente actualizada correctamente.",
            Data = id
        });
    }
}