using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Interface;
using SistemaHorarios.Logica.Negocio.Usuario.Interface;
using SistemaHorarios.Modelos.DTOs.Usuarios;

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

    [HttpPost]
    public async Task<IActionResult> Crear(CrearUsuarioDto dto)
    {
        var usuario = await _usuarioService.CrearUsuarioAsync(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = usuario.IdUsuario },
            usuario);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var usuario = await _usuarioService.ObtenerPorIdAsync(id);

        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var usuarios = await _usuarioService.ObtenerTodosAsync();

        return Ok(usuarios);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarUsuarioDto dto)
    {
        var actualizado =
            await _usuarioService.ActualizarUsuarioAsync(id, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _usuarioService.EliminarUsuarioAsync(id);

        if (!eliminado)
            return NotFound();

        return NoContent();
    }

    [Authorize]
    [HttpGet("perfil")]
    public async Task<IActionResult> Perfil()
    {
        var idUsuario =
            int.Parse(User.FindFirst("id")!.Value);

        var perfil =
            await _usuarioService.ObtenerPerfilAsync(idUsuario);

        return Ok(perfil);
    }

    [Authorize]
    [HttpPut("perfil")]
    public async Task<IActionResult> ActualizarPerfil(
        ActualizarPerfilDto dto)
    {
        var idUsuario =
            int.Parse(User.FindFirst("id")!.Value);

        var actualizado =
            await _usuarioService.ActualizarPerfilAsync(idUsuario, dto);

        if (!actualizado)
            return NotFound();

        return NoContent();
    }

    [Authorize]
    [HttpPut("cambiar-contrasena")]
    public async Task<IActionResult> CambiarContrasena(
        CambiarContrasenaDto dto)
    {
        var idUsuario =
            int.Parse(User.FindFirst("id")!.Value);

        var cambiado =
            await _usuarioService.CambiarContrasenaAsync(idUsuario, dto);

        if (!cambiado)
            return BadRequest();

        return NoContent();
    }

    [HttpPost("verificar")]
    public async Task<IActionResult> Verificar(
        VerificarUsuarioDto dto)
    {
        var resultado =
            await _usuarioService.VerificarUsuarioAsync(dto);

        return Ok(resultado);
    }
}