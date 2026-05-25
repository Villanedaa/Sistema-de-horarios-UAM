using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Modelos.DTOs.Auth;
using SistemaHorarios.Modelos.Responses;
using System.Security.Claims;

namespace SistemaHorarios.API.Controllers;

/// <summary>
/// Controlador encargado de la autenticación y gestión de acceso al sistema.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Inicializa el controlador de autenticación.
    /// </summary>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    [HttpPost("registrar")]
    public async Task<ActionResult<ApiResponse<object>>> Registrar(
        [FromBody] RegistroUsuarioDto dto)
    {
        await _authService.Registrar(dto);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Usuario registrado correctamente.",
            Data = null
        });
    }

    /// <summary>
    /// Permite iniciar sesión y obtener un token JWT.
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(
        [FromBody] LoginRequestDto dto)
    {
        LoginResponseDto resultado = await _authService.Login(dto);

        return Ok(new ApiResponse<LoginResponseDto>
        {
            Success = true,
            Message = "Login exitoso.",
            Data = resultado
        });
    }

    /// <summary>
    /// Obtiene el perfil del usuario autenticado.
    /// </summary>
    [Authorize]
    [HttpGet("perfil")]
    public async Task<ActionResult<ApiResponse<PerfilUsuarioDto>>> Perfil()
    {
        int? idUsuario = ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized(new ApiResponse<PerfilUsuarioDto>
            {
                Success = false,
                Message = "Token inválido.",
                Data = null
            });
        }

        PerfilUsuarioDto perfil = await _authService.ObtenerPerfil(idUsuario.Value);

        return Ok(new ApiResponse<PerfilUsuarioDto>
        {
            Success = true,
            Message = "Perfil obtenido correctamente.",
            Data = perfil
        });
    }

    /// <summary>
    /// Obtiene el identificador del usuario desde el token JWT.
    /// </summary>
    private int? ObtenerIdUsuarioDesdeToken()
    {
        string? idUsuarioTexto =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(idUsuarioTexto, out int idUsuario))
        {
            return null;
        }

        return idUsuario;
    }
}