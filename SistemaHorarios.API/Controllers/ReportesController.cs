using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Reportes.Interfaces;
using SistemaHorarios.Modelos.DTOs.Reportes;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Administrador,Coordinador")]
public class ReportesController : ControllerBase
{
    private readonly IReporteService _reporteService;

    public ReportesController(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }

    [HttpGet("general")]
    public async Task<ActionResult<ReporteGeneralDto>> ObtenerReporteGeneral()
    {
        var reporte =
            await _reporteService.ObtenerReporteGeneralAsync();

        return Ok(reporte);
    }

    [HttpGet("usuarios-por-rol")]
    public async Task<ActionResult<List<ReporteUsuariosPorRolDto>>> ObtenerUsuariosPorRol()
    {
        var reporte =
            await _reporteService.ObtenerUsuariosPorRolAsync();

        return Ok(reporte);
    }

    [HttpGet("materias-por-semestre")]
    public async Task<ActionResult<List<ReporteMateriasPorSemestreDto>>> ObtenerMateriasPorSemestre()
    {
        var reporte =
            await _reporteService.ObtenerMateriasPorSemestreAsync();

        return Ok(reporte);
    }

    [HttpGet("franjas-por-dia")]
    public async Task<ActionResult<List<ReporteFranjaHorariaDto>>> ObtenerFranjasPorDia()
    {
        var reporte =
            await _reporteService.ObtenerFranjasPorDiaAsync();

        return Ok(reporte);
    }

    [HttpGet("planes-academicos")]
    public async Task<ActionResult<List<ReportePlanAcademicoDto>>> ObtenerPlanesAcademicos()
    {
        var reporte =
            await _reporteService.ObtenerPlanesAcademicosAsync();

        return Ok(reporte);
    }
}
