using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Reportes.Interfaces;
using SistemaHorarios.Modelos.DTOs.Reportes;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador,Coordinador")]
[AllowAnonymous]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReportesController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    [HttpGet("general")]
    public async Task<ActionResult<ApiResponse<ReporteGeneralDto>>> ObtenerReporteGeneral()
    {
        var reporte = await _reporteService.ObtenerReporteGeneralAsync();

        return Ok(new ApiResponse<ReporteGeneralDto>
        {
            Success = true,
            Message = "Reporte general consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("usuarios-por-rol")]
    public async Task<ActionResult<ApiResponse<List<ReporteUsuariosPorRolDto>>>> ObtenerUsuariosPorRol()
    {
        var reporte = await _reporteService.ObtenerUsuariosPorRolAsync();

        return Ok(new ApiResponse<List<ReporteUsuariosPorRolDto>>
        {
            Success = true,
            Message = "Reporte de usuarios por rol consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("materias-por-semestre")]
    public async Task<ActionResult<ApiResponse<List<ReporteMateriasPorSemestreDto>>>> ObtenerMateriasPorSemestre()
    {
        var reporte = await _reporteService.ObtenerMateriasPorSemestreAsync();

        return Ok(new ApiResponse<List<ReporteMateriasPorSemestreDto>>
        {
            Success = true,
            Message = "Reporte de materias por semestre consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("franjas-por-dia")]
    public async Task<ActionResult<ApiResponse<List<ReporteFranjaHorariaDto>>>> ObtenerFranjasPorDia()
    {
        var reporte = await _reporteService.ObtenerFranjasPorDiaAsync();

        return Ok(new ApiResponse<List<ReporteFranjaHorariaDto>>
        {
            Success = true,
            Message = "Reporte de franjas por día consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("planes-academicos")]
    public async Task<ActionResult<ApiResponse<List<ReportePlanAcademicoDto>>>> ObtenerPlanesAcademicos()
    {
        var reporte = await _reporteService.ObtenerPlanesAcademicosAsync();

        return Ok(new ApiResponse<List<ReportePlanAcademicoDto>>
        {
            Success = true,
            Message = "Reporte de planes académicos consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("horarios-generados")]
    public async Task<ActionResult<ApiResponse<List<ReporteHorarioGeneradoDto>>>> ObtenerHorariosGenerados()
    {
        var reporte = await _reporteService.ObtenerHorariosGeneradosAsync();

        return Ok(new ApiResponse<List<ReporteHorarioGeneradoDto>>
        {
            Success = true,
            Message = "Reporte de horarios generados consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("horario-grupo/{idGrupo:int}")]
    public async Task<ActionResult<ApiResponse<List<ReporteHorarioGrupoDto>>>> ObtenerHorarioPorGrupo(
        int idGrupo)
    {
        var reporte = await _reporteService.ObtenerHorarioPorGrupoAsync(idGrupo);

        return Ok(new ApiResponse<List<ReporteHorarioGrupoDto>>
        {
            Success = true,
            Message = "Reporte de horario por grupo consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("horario-docente/{idDocente:int}")]
    public async Task<ActionResult<ApiResponse<List<ReporteHorarioDocenteDto>>>> ObtenerHorarioPorDocente(
        int idDocente)
    {
        var reporte = await _reporteService.ObtenerHorarioPorDocenteAsync(idDocente);

        return Ok(new ApiResponse<List<ReporteHorarioDocenteDto>>
        {
            Success = true,
            Message = "Reporte de horario por docente consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("carga-docente")]
    public async Task<ActionResult<ApiResponse<List<ReporteCargaDocenteDto>>>> ObtenerCargaDocente()
    {
        var reporte = await _reporteService.ObtenerCargaDocenteAsync();

        return Ok(new ApiResponse<List<ReporteCargaDocenteDto>>
        {
            Success = true,
            Message = "Reporte de carga docente consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("conflictos-horarios")]
    public async Task<ActionResult<ApiResponse<List<ReporteConflictoHorarioDto>>>> ObtenerConflictosHorario()
    {
        var reporte = await _reporteService.ObtenerConflictosHorarioAsync();

        return Ok(new ApiResponse<List<ReporteConflictoHorarioDto>>
        {
            Success = true,
            Message = "Reporte de conflictos de horario consultado correctamente.",
            Data = reporte
        });
    }

    [HttpGet("descargar")]
    public async Task<IActionResult> DescargarReporte(
        [FromQuery] string tipo,
        [FromQuery] string formato = "csv",
        [FromQuery] int? idGrupo = null,
        [FromQuery] int? idDocente = null)
    {
        try
        {
            var archivo = await _reporteService.ExportarReporteAsync(
                tipo,
                formato,
                idGrupo,
                idDocente);

            return File(
                archivo.Archivo,
                archivo.ContentType,
                archivo.NombreArchivo);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}
