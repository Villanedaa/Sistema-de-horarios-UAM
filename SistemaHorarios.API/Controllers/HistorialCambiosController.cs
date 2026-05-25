using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/historial-cambios")]
public class HistorialCambiosController : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<List<object>>> ObtenerHistorialCambios()
    {
        return Ok(new ApiResponse<List<object>>
        {
            Success = false,
            Message = "El módulo de historial de cambios está deshabilitado.",
            Data = new List<object>()
        });
    }

    [HttpGet("modulos")]
    public ActionResult<ApiResponse<List<string>>> ObtenerModulos()
    {
        return Ok(new ApiResponse<List<string>>
        {
            Success = true,
            Message = "Historial deshabilitado.",
            Data = new List<string>()
        });
    }

    [HttpGet("acciones")]
    public ActionResult<ApiResponse<List<string>>> ObtenerAcciones()
    {
        return Ok(new ApiResponse<List<string>>
        {
            Success = true,
            Message = "Historial deshabilitado.",
            Data = new List<string>()
        });
    }

    [HttpGet("usuarios")]
    public ActionResult<ApiResponse<List<string>>> ObtenerUsuarios()
    {
        return Ok(new ApiResponse<List<string>>
        {
            Success = true,
            Message = "Historial deshabilitado.",
            Data = new List<string>()
        });
    }
}
