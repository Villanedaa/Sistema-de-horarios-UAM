using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/historial-cambios")]
public class HistorialCambiosController : ControllerBase
{
    private record CambioItem(int IdCambio, string Fecha, string Hora, string Usuario, string Modulo, string Descripcion);

    private static readonly CambioItem[] _datos =
    {
        new(1, "2026-04-27", "07:26", "admin.horarios", "Horarios",  "Actualizó límite de créditos nocturno para validación"),
        new(2, "2026-04-27", "10:45", "coord.sistemas",  "Horarios",  "Generó propuesta PROP-2026-01-A"),
        new(3, "2026-04-27", "15:40", "admin.horarios",  "Docentes",  "Registró disponibilidad de Marcela"),
        new(4, "2026-04-28", "09:10", "admin.general",   "Usuarios",  "Creó cuenta para nuevo coordinador"),
        new(5, "2026-04-28", "11:30", "coord.sistemas",  "Horarios",  "Aprobó propuesta PROP-2026-01-A")
    };

    [HttpGet]
    public IActionResult ObtenerHistorialCambios(
        [FromQuery] string? usuario,
        [FromQuery] string? modulo,
        [FromQuery] string? fechaDesde,
        [FromQuery] string? fechaHasta)
    {
        IEnumerable<CambioItem> query = _datos;

        if (!string.IsNullOrWhiteSpace(usuario))
            query = query.Where(x => x.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(modulo))
            query = query.Where(x => x.Modulo.Equals(modulo, StringComparison.OrdinalIgnoreCase));

        if (DateTime.TryParse(fechaDesde, out var desde))
            query = query.Where(x => DateTime.Parse($"{x.Fecha} {x.Hora}").Date >= desde.Date);

        if (DateTime.TryParse(fechaHasta, out var hasta))
            query = query.Where(x => DateTime.Parse($"{x.Fecha} {x.Hora}").Date <= hasta.Date);

        return Ok(query.OrderByDescending(x => x.Fecha).ThenByDescending(x => x.Hora).ToArray());
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerCambioPorId(int id)
    {
        var item = _datos.FirstOrDefault(x => x.IdCambio == id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpGet("modulos")]
    public IActionResult ObtenerModulos()
        => Ok(_datos.Select(x => x.Modulo).Distinct().OrderBy(m => m).ToArray());

    [HttpGet("usuarios")]
    public IActionResult ObtenerUsuarios()
        => Ok(_datos.Select(x => x.Usuario).Distinct().OrderBy(u => u).ToArray());
}
