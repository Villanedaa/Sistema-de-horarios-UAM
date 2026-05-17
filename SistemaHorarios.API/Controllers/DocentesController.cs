using Microsoft.AspNetCore.Mvc;

using SistemaHorarios.Logica.Negocio.Docentes;

using SistemaHorarios.Modelos.DTOs.Docentes;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocentesController : ControllerBase
{
    private readonly IGestorDocente _gestor;

    private readonly IGestorDisponibilidadDocente
        _gestorDisponibilidad;

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

        return Ok(docentes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var docente = await _gestor.ObtenerPorIdAsync(id);

        if (docente == null)
            return NotFound();

        return Ok(docente);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(
        DocenteRequest request)
    {
        var docente = await _gestor.CrearAsync(request);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = docente.IdDocente },
            docente
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        int id,

        DocenteRequest request)
    {
        await _gestor.ActualizarAsync(id, request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _gestor.EliminarAsync(id);

        return NoContent();
    }

    // =====================================================
    // DISPONIBILIDAD DOCENTE
    // =====================================================

    [HttpGet("{id}/disponibilidad")]
    public async Task<IActionResult>
        ObtenerDisponibilidad(int id)
    {
        var disponibilidad =
            await _gestorDisponibilidad
                .ObtenerDisponibilidadAsync(id);

        return Ok(disponibilidad);
    }

    [HttpPut("{id}/disponibilidad")]
    public async Task<IActionResult>
        ActualizarDisponibilidad(
            int id,

            ActualizarDisponibilidadDocenteRequest request)
    {
        await _gestorDisponibilidad
            .ActualizarDisponibilidadAsync(id, request);

        return NoContent();
    }
}