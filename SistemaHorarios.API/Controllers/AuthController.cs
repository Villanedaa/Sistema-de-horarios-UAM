using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Modelos.DTOs.Auth;
using System.Security.Claims;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(
        IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(
    RegistroUsuarioDto dto)
    {
        await _authService.Registrar(dto);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,

                Message ="Usuario registrado correctamente"
            });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto dto)
    {
        var resultado =
            await _authService.Login(dto);

        return Ok(
            new ApiResponse<LoginResponseDto>
            {
                Success = true,

                Message = "Login exitoso",

                Data = resultado
            });
    }

    [Authorize]
    [HttpGet("perfil")]
    public async Task<IActionResult> Perfil()
    {
        var idUsuario =
            int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )!.Value
            );

        var perfil =
            await _authService
                .ObtenerPerfil(idUsuario);

        return Ok(
            new ApiResponse<PerfilUsuarioDto>
            {
                Success = true,

                Message = "Perfil obtenido",

                Data = perfil
            });
    }
}