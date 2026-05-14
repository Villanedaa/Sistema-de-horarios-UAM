using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Modelos.DTOs.Auth;

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

        return Ok(new
        {
            mensaje =
                "Usuario registrado correctamente"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        LoginRequestDto dto)
    {
        var resultado =
            await _authService.Login(dto);

        return Ok(resultado);
    }
}