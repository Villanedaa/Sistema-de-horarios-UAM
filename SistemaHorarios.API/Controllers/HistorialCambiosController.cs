using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/historial-cambios")]
public class HistorialCambiosController : ControllerBase
{
    [HttpGet]
    public IActionResult ObtenerHistorialCambios(
        [FromQuery] string? usuario,
        [FromQuery] string? modulo,
        [FromQuery] string? fechaDesde,
        [FromQuery] string? fechaHasta)
    {
        return Ok(new[]
        {
            new
            {
                IdCambio = 1,
                Fecha = "2026-04-27",
                Hora = "07:26",
                Usuario = "admin.horarios",
                Modulo = "Horarios",
                Descripcion = "Actualizó límite de créditos nocturno para validación"
            },
            new
            {
                IdCambio = 2,
                Fecha = "2026-04-27",
                Hora = "10:45",
                Usuario = "coord.sistemas",
                Modulo = "Horarios",
                Descripcion = "Generó propuesta PROP-2026-01-A"
            },
            new
            {
                IdCambio = 3,
                Fecha = "2026-04-27",
                Hora = "15:40",
                Usuario = "admin.horarios",
                Modulo = "Docentes",
                Descripcion = "Registró disponibilidad de Marcela"
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerCambioPorId(int id)
    {
        return Ok(new
        {
            IdCambio = id,
            Fecha = "2026-04-27",
            Hora = "07:26",
            Usuario = "admin.horarios",
            Modulo = "Horarios",
            Descripcion = "Actualizó límite de créditos nocturno para validación",
            DetalleAnterior = "Límite anterior: 16 créditos",
            DetalleNuevo = "Nuevo límite: 18 créditos"
        });
    }

    [HttpGet("modulos")]
    public IActionResult ObtenerModulos()
    {
        return Ok(new[]
        {
            "Plan académico",
            "Docentes",
            "Horarios",
            "Materias",
            "Reportes",
            "Grupos académicos",
            "Usuarios"
        });
    }

    [HttpGet("usuarios")]
    public IActionResult ObtenerUsuarios()
    {
        return Ok(new[]
        {
            "admin.horarios",
            "coord.sistemas",
            "admin.general"
        });
    }
}