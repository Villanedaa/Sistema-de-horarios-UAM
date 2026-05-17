using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Roles.Interfaces;
using SistemaHorarios.Modelos.DTOs.Roles;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly IRolService _rolService;

    public RolesController(
        IRolService rolService)
    {
        _rolService = rolService;
    }

    // Obtiene todos los roles
    [HttpGet]
    public async Task<IActionResult> ObtenerRoles()
    {
        var roles =
            await _rolService.ObtenerRoles();

        return Ok(
            new ApiResponse<List<RolResponseDto>>
            {
                Success = true,

                Message = "Roles obtenidos correctamente",

                Data = roles
            });
    }

    // Obtiene un rol por Id
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(
        int id)
    {
        var rol =
            await _rolService
                .ObtenerPorId(id);

        if (rol == null)
        {
            return NotFound(
                new ApiResponse<object>
                {
                    Success = false,

                    Message = "Rol no encontrado"
                });
        }

        return Ok(
            new ApiResponse<RolResponseDto>
            {
                Success = true,

                Message = "Rol obtenido correctamente",

                Data = rol
            });
    }

    // Crea un nuevo rol
    [Authorize(Policy = "SoloAdministradores")]
    [HttpPost]
    public async Task<IActionResult> CrearRol(
        CrearRolDto dto)
    {
        await _rolService.CrearRol(dto);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,

                Message = "Rol creado correctamente"
            });
    }

    // Actualiza un rol existente
    [Authorize(Policy = "SoloAdministradores")]
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarRol(
        int id,
        ActualizarRolDto dto)
    {
        await _rolService
            .ActualizarRol(id, dto);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,

                Message = "Rol actualizado correctamente"
            });
    }

    // Elimina un rol
    [Authorize(Policy = "SoloAdministradores")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarRol(
        int id)
    {
        await _rolService.EliminarRol(id);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,

                Message = "Rol eliminado correctamente"
            });
    }
}