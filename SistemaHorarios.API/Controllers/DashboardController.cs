using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Dashboard.Interfaces;
using SistemaHorarios.Modelos.DTOs.Dashboard;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("resumen")]
    public async Task<ActionResult<ApiResponse<DashboardResumenDto>>> ObtenerResumen()
    {
        var resumen = await _dashboardService.ObtenerResumenAsync();

        return Ok(new ApiResponse<DashboardResumenDto>
        {
            Success = true,
            Message = "Resumen del dashboard consultado correctamente.",
            Data = resumen
        });
    }
}