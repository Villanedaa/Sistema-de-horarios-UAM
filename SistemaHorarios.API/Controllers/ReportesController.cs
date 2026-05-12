using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/reportes")]
public class ReportesController : ControllerBase
{
    [HttpGet]
    public IActionResult ObtenerReportes([FromQuery] string? tipo, [FromQuery] string? fecha)
    {
        return Ok(new[]
        {
            new
            {
                IdReporte = 1,
                Fecha = "2026-04-21",
                Tipo = "Carga docente",
                Usuario = "admin",
                Detalle = "Diurna",
                FormatosDisponibles = new[] { "PDF", "CSV" }
            },
            new
            {
                IdReporte = 2,
                Fecha = "2026-04-21",
                Tipo = "Horarios generados",
                Usuario = "coordinador",
                Detalle = "Nocturna",
                FormatosDisponibles = new[] { "PDF", "CSV" }
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerReportePorId(int id)
    {
        return Ok(new
        {
            IdReporte = id,
            Fecha = "2026-04-21",
            Tipo = "Carga docente",
            Usuario = "admin",
            Detalle = "Diurna",
            FormatosDisponibles = new[] { "PDF", "CSV" }
        });
    }

    [HttpPost]
    public IActionResult CrearReporte([FromBody] CrearReporteRequest request)
    {
        return Ok(new
        {
            IdReporte = 3,
            request.TipoReporte,
            request.Periodo,
            request.Fecha,
            request.FormatoInicial,
            request.Descripcion,
            Mensaje = "Reporte generado correctamente."
        });
    }

    [HttpGet("{id}/vista-previa")]
    public IActionResult ObtenerVistaPreviaReporte(int id)
    {
        return Ok(new
        {
            IdReporte = id,
            TipoReporte = "CargaDocente",
            Columnas = new[]
            {
                "Docente",
                "Materia",
                "Grupo",
                "Jornada",
                "Créditos",
                "Rango"
            },
            Filas = new[]
            {
                new
                {
                    Docente = "Diana",
                    Materia = "Ing. Software II",
                    Grupo = "01",
                    Jornada = "Diurna",
                    Creditos = 3,
                    Rango = "Lunes 10:00 - 12:00"
                }
            }
        });
    }

    [HttpGet("{id}/descargar")]
    public IActionResult DescargarReporte(int id, [FromQuery] string formato)
    {
        return Ok(new
        {
            IdReporte = id,
            Formato = formato.ToUpper(),
            UrlDescarga = $"/archivos/reportes/reporte-{id}.{formato.ToLower()}",
            Mensaje = "Reporte descargado correctamente."
        });
    }

    [HttpGet("tipos")]
    public IActionResult ObtenerTiposReporte()
    {
        return Ok(new[]
        {
            new
            {
                Codigo = "CargaDocente",
                Nombre = "Carga docente"
            },
            new
            {
                Codigo = "HorariosGenerados",
                Nombre = "Horarios generados"
            },
            new
            {
                Codigo = "AsignacionPorGrupo",
                Nombre = "Asignación por grupo"
            },
            new
            {
                Codigo = "ConflictosHorario",
                Nombre = "Conflictos de horario"
            }
        });
    }
}

public class CrearReporteRequest
{
    public string TipoReporte { get; set; } = string.Empty;
    public string Periodo { get; set; } = string.Empty;
    public string Fecha { get; set; } = string.Empty;
    public string FormatoInicial { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}