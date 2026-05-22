using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/historial-cambios")]
[Authorize(Roles = "Administrador")]
public class HistorialCambiosController : ControllerBase
{
    public record CambioItem(
        int IdCambio,
        string Fecha,
        string Hora,
        string Usuario,
        string Modulo,
        string Descripcion);

    private static readonly CambioItem[] _datos =
    {
        new(1, "2026-04-27", "07:26", "admin.horarios", "Horarios", "Actualizó límite de créditos nocturno para validación"),
        new(2, "2026-04-27", "10:45", "coord.sistemas", "Horarios", "Generó propuesta PROP-2026-01-A"),
        new(3, "2026-04-27", "15:40", "admin.horarios", "Docentes", "Registró disponibilidad de Marcela"),
        new(4, "2026-04-28", "09:10", "admin.general", "Usuarios", "Creó cuenta para nuevo coordinador"),
        new(5, "2026-04-28", "11:30", "coord.sistemas", "Horarios", "Aprobó propuesta PROP-2026-01-A")
    };

    [HttpGet]
    public ActionResult<ApiResponse<CambioItem[]>> ObtenerHistorialCambios(
        [FromQuery] string? usuario,
        [FromQuery] string? modulo,
        [FromQuery] string? fechaDesde,
        [FromQuery] string? fechaHasta)
    {
        IEnumerable<CambioItem> query = _datos;

        if (!string.IsNullOrWhiteSpace(usuario))
        {
            query = query.Where(x =>
                x.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(modulo))
        {
            query = query.Where(x =>
                x.Modulo.Equals(modulo, StringComparison.OrdinalIgnoreCase));
        }

        if (DateTime.TryParse(fechaDesde, out var desde))
        {
            query = query.Where(x =>
                DateTime.Parse($"{x.Fecha} {x.Hora}").Date >= desde.Date);
        }

        if (DateTime.TryParse(fechaHasta, out var hasta))
        {
            query = query.Where(x =>
                DateTime.Parse($"{x.Fecha} {x.Hora}").Date <= hasta.Date);
        }

        CambioItem[] resultado = query
            .OrderByDescending(x => x.Fecha)
            .ThenByDescending(x => x.Hora)
            .ToArray();

        return Ok(new ApiResponse<CambioItem[]>
        {
            Success = true,
            Message = "Historial de cambios consultado correctamente.",
            Data = resultado
        });
    }

    [HttpGet("{id}")]
    public ActionResult<ApiResponse<CambioItem>> ObtenerCambioPorId(int id)
    {
        CambioItem? item = _datos.FirstOrDefault(x => x.IdCambio == id);

        if (item == null)
        {
            return NotFound(new ApiResponse<CambioItem>
            {
                Success = false,
                Message = "Cambio no encontrado.",
                Data = null
            });
        }

        return Ok(new ApiResponse<CambioItem>
        {
            Success = true,
            Message = "Cambio consultado correctamente.",
            Data = item
        });
    }

    [HttpGet("modulos")]
    public ActionResult<ApiResponse<string[]>> ObtenerModulos()
    {
        string[] modulos = _datos
            .Select(x => x.Modulo)
            .Distinct()
            .OrderBy(m => m)
            .ToArray();

        return Ok(new ApiResponse<string[]>
        {
            Success = true,
            Message = "Módulos del historial consultados correctamente.",
            Data = modulos
        });
    }

    [HttpGet("usuarios")]
    public ActionResult<ApiResponse<string[]>> ObtenerUsuarios()
    {
        string[] usuarios = _datos
            .Select(x => x.Usuario)
            .Distinct()
            .OrderBy(u => u)
            .ToArray();

        return Ok(new ApiResponse<string[]>
        {
            Success = true,
            Message = "Usuarios del historial consultados correctamente.",
            Data = usuarios
        });
    }
}