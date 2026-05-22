using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Usuario.Interface;
using SistemaHorarios.Modelos.DTOs.Usuarios;
using SistemaHorarios.Modelos.Responses;
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
    public async Task<ActionResult<ApiResponse<IEnumerable<UsuarioResponseDto>>>> ObtenerTodos()
    {
        var usuarios = await _usuarioService.ObtenerTodosAsync();

        return Ok(new ApiResponse<IEnumerable<UsuarioResponseDto>>
        {
            Success = true,
            Message = "Usuarios consultados correctamente.",
            Data = usuarios
        });
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> ObtenerPorId(int id)
    {
        var usuario = await _usuarioService.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            return NotFound(new ApiResponse<UsuarioResponseDto>
            {
                Success = false,
                Message = "Usuario no encontrado.",
                Data = null
            });
        }

        return Ok(new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Usuario consultado correctamente.",
            Data = usuario
        });
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> Crear(
        CrearUsuarioDto dto)
    {
        var usuarioCreado = await _usuarioService.CrearUsuarioAsync(dto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = usuarioCreado.IdUsuario },
            new ApiResponse<UsuarioResponseDto>
            {
                Success = true,
                Message = "Usuario creado correctamente.",
                Data = usuarioCreado
            }
        );
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ApiResponse<int>>> Actualizar(
        int id,
        ActualizarUsuarioDto dto)
    {
        bool actualizado = await _usuarioService.ActualizarUsuarioAsync(id, dto);

        if (!actualizado)
        {
            return NotFound(new ApiResponse<int>
            {
                Success = false,
                Message = "Usuario no encontrado.",
                Data = id
            });
        }

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Usuario actualizado correctamente.",
            Data = id
        });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult<ApiResponse<int>>> Eliminar(int id)
    {
        bool eliminado = await _usuarioService.EliminarUsuarioAsync(id);

        if (!eliminado)
        {
            return NotFound(new ApiResponse<int>
            {
                Success = false,
                Message = "Usuario no encontrado.",
                Data = id
            });
        }

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Usuario eliminado correctamente.",
            Data = id
        });
    }

    [HttpGet("perfil")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> ObtenerPerfil()
    {
        int? idUsuario = ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized(new ApiResponse<UsuarioResponseDto>
            {
                Success = false,
                Message = "Token inválido.",
                Data = null
            });
        }

        var perfil = await _usuarioService.ObtenerPerfilAsync(idUsuario.Value);

        if (perfil == null)
        {
            return NotFound(new ApiResponse<UsuarioResponseDto>
            {
                Success = false,
                Message = "Usuario no encontrado.",
                Data = null
            });
        }

        return Ok(new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Perfil consultado correctamente.",
            Data = perfil
        });
    }

    [HttpPut("perfil")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<int>>> ActualizarPerfil(
        ActualizarPerfilDto dto)
    {
        int? idUsuario = ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized(new ApiResponse<int>
            {
                Success = false,
                Message = "Token inválido.",
                Data = 0
            });
        }

        bool actualizado = await _usuarioService.ActualizarPerfilAsync(
            idUsuario.Value,
            dto);

        if (!actualizado)
        {
            return NotFound(new ApiResponse<int>
            {
                Success = false,
                Message = "Usuario no encontrado.",
                Data = idUsuario.Value
            });
        }

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Perfil actualizado correctamente.",
            Data = idUsuario.Value
        });
    }

    [HttpPut("cambiar-contrasena")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<int>>> CambiarContrasena(
        CambiarContrasenaDto dto)
    {
        int? idUsuario = ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized(new ApiResponse<int>
            {
                Success = false,
                Message = "Token inválido.",
                Data = 0
            });
        }

        bool actualizada = await _usuarioService.CambiarContrasenaAsync(
            idUsuario.Value,
            dto);

        if (!actualizada)
        {
            return BadRequest(new ApiResponse<int>
            {
                Success = false,
                Message = "No se pudo cambiar la contraseña. Verifica la contraseña actual.",
                Data = idUsuario.Value
            });
        }

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Contraseña actualizada correctamente.",
            Data = idUsuario.Value
        });
    }

    [HttpPost("verificar")]
    public async Task<ActionResult<ApiResponse<VerificarUsuarioResponseDto>>> VerificarUsuario(
        VerificarUsuarioDto dto)
    {
        var resultado = await _usuarioService.VerificarUsuarioAsync(dto);

        return Ok(new ApiResponse<VerificarUsuarioResponseDto>
        {
            Success = true,
            Message = "Verificación de usuario realizada correctamente.",
            Data = resultado
        });
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