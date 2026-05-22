using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Reportes.Interfaces;
using SistemaHorarios.Modelos.DTOs.Reportes;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
}