using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Usuario.Interface;
using SistemaHorarios.Modelos.DTOs.Usuarios;
using System.Security.Claims;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> ObtenerTodos()
    {
        var usuarios =
            await _usuarioService.ObtenerTodosAsync();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<UsuarioResponseDto>> ObtenerPorId(int id)
    {
        var usuario =
            await _usuarioService.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        return Ok(usuario);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<UsuarioResponseDto>> Crear(
        CrearUsuarioDto dto)
    {
        var usuarioCreado =
            await _usuarioService.CrearUsuarioAsync(dto);

        return Ok(usuarioCreado);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult> Actualizar(
        int id,
        ActualizarUsuarioDto dto)
    {
        bool actualizado =
            await _usuarioService.ActualizarUsuarioAsync(id, dto);

        if (!actualizado)
        {
            return NotFound("Usuario no encontrado.");
        }

        return Ok("Usuario actualizado correctamente.");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult> Eliminar(int id)
    {
        bool eliminado =
            await _usuarioService.EliminarUsuarioAsync(id);

        if (!eliminado)
        {
            return NotFound("Usuario no encontrado.");
        }

        return Ok("Usuario eliminado correctamente.");
    }

    [HttpGet("perfil")]
    [Authorize]
    public async Task<ActionResult<UsuarioResponseDto>> ObtenerPerfil()
    {
        int? idUsuario =
            ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized("Token inválido.");
        }

        var perfil =
            await _usuarioService.ObtenerPerfilAsync(idUsuario.Value);

        if (perfil == null)
        {
            return NotFound("Usuario no encontrado.");
        }

        return Ok(perfil);
    }

    [HttpPut("perfil")]
    [Authorize]
    public async Task<ActionResult> ActualizarPerfil(
        ActualizarPerfilDto dto)
    {
        int? idUsuario =
            ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized("Token inválido.");
        }

        bool actualizado =
            await _usuarioService.ActualizarPerfilAsync(
                idUsuario.Value,
                dto);

        if (!actualizado)
        {
            return NotFound("Usuario no encontrado.");
        }

        return Ok("Perfil actualizado correctamente.");
    }

    [HttpPut("cambiar-contrasena")]
    [Authorize]
    public async Task<ActionResult> CambiarContrasena(
        CambiarContrasenaDto dto)
    {
        int? idUsuario =
            ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized("Token inválido.");
        }

        bool actualizada =
            await _usuarioService.CambiarContrasenaAsync(
                idUsuario.Value,
                dto);

        if (!actualizada)
        {
            return BadRequest(
                "No se pudo cambiar la contraseña. Verifica la contraseña actual.");
        }

        return Ok("Contraseña actualizada correctamente.");
    }

    [HttpPost("verificar")]
    public async Task<ActionResult<VerificarUsuarioResponseDto>> VerificarUsuario(
        VerificarUsuarioDto dto)
    {
        var resultado =
            await _usuarioService.VerificarUsuarioAsync(dto);

        return Ok(resultado);
    }

    private int? ObtenerIdUsuarioDesdeToken()
    {
        var idUsuarioTexto =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(idUsuarioTexto, out int idUsuario))
        {
            return null;
        }

        return idUsuario;
    }
}