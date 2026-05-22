using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/catalogos")]
[Authorize(Roles = "Administrador,Coordinador")]
public class CatalogosController : ControllerBase
{
    [HttpGet("jornadas")]
    public IActionResult ObtenerJornadas()
    {
        return Ok(new[]
        {
            "Diurna",
            "Nocturna"
        });
    }

    [HttpGet("estados")]
    public IActionResult ObtenerEstados()
    {
        return Ok(new[]
        {
            "Activo",
            "Inactivo",
            "Pendiente",
            "Aprobado",
            "Rechazado",
            "Generado"
        });
    }

    [HttpGet("roles")]
    public IActionResult ObtenerRoles()
    {
        return Ok(new[]
        {
            "Admin",
            "Coordinador"
        });
    }

    [HttpGet("dias")]
    public IActionResult ObtenerDias()
    {
        return Ok(new[]
        {
            "Lunes",
            "Martes",
            "Miércoles",
            "Jueves",
            "Viernes",
            "Sábado"
        });
    }

    [HttpGet("tipos-grupo")]
    public IActionResult ObtenerTiposGrupo()
    {
        return Ok(new[]
        {
            "Regular",
            "TAPSI",
            "Mixto"
        });
    }

    [HttpGet("tipos-reporte")]
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

    [HttpGet("formatos-reporte")]
    public IActionResult ObtenerFormatosReporte()
    {
        return Ok(new[]
        {
            "PDF",
            "CSV"
        });
    }

    [HttpGet("semestres")]
    public IActionResult ObtenerSemestres([FromQuery] string? jornada)
    {
        int totalSemestres = jornada?.ToLower() == "nocturna" ? 12 : 10;

        var semestres = new List<int>();

        for (int i = 1; i <= totalSemestres; i++)
        {
            semestres.Add(i);
        }

        return Ok(semestres);
    }

    [HttpGet("modulos")]
    public IActionResult ObtenerModulos()
    {
        return Ok(new[]
        {
            "Inicio",
            "Plan Académico",
            "Materias",
            "Docentes",
            "Grupos Académicos",
            "Horarios",
            "Reportes",
            "Coordinadores",
            "Historial de cambios"
        });
    }
}