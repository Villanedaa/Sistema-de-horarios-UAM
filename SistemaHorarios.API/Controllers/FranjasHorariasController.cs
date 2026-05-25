using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.FranjasHorarias.Interfaces;
using SistemaHorarios.Modelos.DTOs.FranjasHorarias;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/franjas-horarias")]
[Authorize(Roles = "Administrador,Coordinador")]
public class FranjasHorariasController
    : ControllerBase
{
    private readonly IFranjaHorariaService
        _franjaHorariaService;

    public FranjasHorariasController(
        IFranjaHorariaService
            franjaHorariaService)
    {
        _franjaHorariaService =
            franjaHorariaService;
    }

    // Obtiene todas las franjas horarias
    [HttpGet]
    public async Task<IActionResult>
        ObtenerFranjasHorarias()
    {
        var franjas =
            await _franjaHorariaService
                .ObtenerFranjasHorarias();

        return Ok(
            new ApiResponse
                <List<FranjaHorariaResponseDto>>
            {
                Success = true,

                Message =
                    "Franjas horarias obtenidas correctamente",

                Data = franjas
            });
    }

    // Obtiene una franja horaria por Id
    [HttpGet("{id}")]
    public async Task<IActionResult>
        ObtenerPorId(
            int id)
    {
        var franja =
            await _franjaHorariaService
                .ObtenerPorId(id);

        if (franja == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,

                    Message =
                        "Franja horaria no encontrada"
                });
        }

        return Ok(
            new ApiResponse
                <FranjaHorariaResponseDto>
            {
                Success = true,

                Message =
                    "Franja horaria obtenida correctamente",

                Data = franja
            });
    }

    // Crea una nueva franja horaria
    [Authorize(Policy = "GestionMaterias")]
    [HttpPost]
    public async Task<IActionResult>
        CrearFranjaHoraria(
            CrearFranjaHorariaDto dto)
    {
        await _franjaHorariaService
            .CrearFranjaHoraria(dto);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,

                Message =
                    "Franja horaria creada correctamente"
            });
    }

    // Actualiza una franja horaria
    [Authorize(Policy = "GestionMaterias")]
    [HttpPut("{id}")]
    public async Task<IActionResult>
        ActualizarFranjaHoraria(
            int id,
            ActualizarFranjaHorariaDto dto)
    {
        await _franjaHorariaService
            .ActualizarFranjaHoraria(
                id,
                dto
            );

        return Ok(
            new ApiResponse<object>
            {
                Success = true,

                Message =
                    "Franja horaria actualizada correctamente"
            });
    }

    // Elimina una franja horaria
    [Authorize(Policy = "GestionMaterias")]
    [HttpDelete("{id}")]
    public async Task<IActionResult>
        EliminarFranjaHoraria(
            int id)
    {
        await _franjaHorariaService
            .EliminarFranjaHoraria(id);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,

                Message =
                    "Franja horaria eliminada correctamente"
            });
    }
}