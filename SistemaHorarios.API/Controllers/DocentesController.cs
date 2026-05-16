using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Docentes;
using SistemaHorarios.Modelos.DTOs.Docentes;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocentesController : ControllerBase
{
    private readonly IGestorDocente _gestor;

    public DocentesController(IGestorDocente gestor)
    {
        _gestor = gestor;
    }

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
    public async Task<IActionResult> Crear(DocenteRequest dto)
    {
        var docente = await _gestor.CrearAsync(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = docente.IdDocente },
            docente
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, DocenteRequest dto)
    {
        await _gestor.ActualizarAsync(id, dto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _gestor.EliminarAsync(id);

        return NoContent();
    }
}