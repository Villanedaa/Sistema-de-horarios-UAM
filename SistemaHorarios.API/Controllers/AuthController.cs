using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Modelos.DTOs.Auth;
using System.Security.Claims;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

/// <summary>
/// Controlador encargado de la autenticación
/// y gestión de acceso al sistema.
///
/// Permite:
/// - registrar usuarios,
/// - iniciar sesión,
/// - obtener perfil autenticado,
/// - generar autenticación JWT.
///
/// Todos los endpoints relacionados con seguridad
/// del sistema se centralizan aquí.
/// </summary>
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    /// <summary>
    /// Servicio encargado de la lógica de autenticación.
    /// </summary>
    private readonly IAuthService _authService;

    /// <summary>
    /// Constructor del controlador de autenticación.
    /// </summary>
    /// <param name="authService">
    /// Servicio de autenticación inyectado mediante
    /// Dependency Injection.
    /// </param>
    public AuthController(
        IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    ///
    /// Este endpoint:
    /// - valida los datos recibidos,
    /// - crea el usuario,
    /// - hashea la contraseña,
    /// - almacena el usuario en base de datos.
    /// </summary>
    /// <param name="dto">
    /// Datos necesarios para registrar el usuario.
    /// </param>
    /// <returns>
    /// Respuesta indicando si el registro fue exitoso.
    /// </returns>
    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar(
        RegistroUsuarioDto dto)
    {
        await _authService.Registrar(dto);

        return Ok(
            new ApiResponse<object>
            {
                Success = true,

                Message = "Usuario registrado correctamente"
            });
    }

    /// <summary>
    /// Permite iniciar sesión en el sistema.
    ///
    /// El endpoint:
    /// - valida credenciales,
    /// - verifica contraseña con BCrypt,
    /// - genera token JWT,
    /// - devuelve información básica del usuario.
    /// </summary>
    /// <param name="dto">
    /// Credenciales de acceso del usuario.
    /// </param>
    /// <returns>
    /// Token JWT e información autenticada.
    /// </returns>
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

    /// <summary>
    /// Obtiene el perfil del usuario autenticado.
    ///
    /// Requiere:
    /// token JWT válido.
    ///
    /// El IdUsuario se obtiene desde los claims
    /// almacenados en el token.
    /// </summary>
    /// <returns>
    /// Información del perfil autenticado.
    /// </returns>
    [Authorize]
    [HttpGet("perfil")]
    public async Task<IActionResult> Perfil()
    {
        // Obtiene el IdUsuario desde el JWT
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