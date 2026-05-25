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
    private readonly IWebHostEnvironment _environment;

    public UsuariosController(
        IUsuarioService usuarioService,
        IWebHostEnvironment environment)
    {
        _usuarioService = usuarioService;
        _environment = environment;
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
    public async Task<ActionResult<ApiResponse<UsuarioResponseDto>>> ObtenerPerfil()
    {
        int? idUsuario =
            ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized(new ApiResponse<UsuarioResponseDto>
            {
                Success = false,
                Message = "Token inválido."
            });
        }

        var perfil =
            await _usuarioService.ObtenerPerfilAsync(idUsuario.Value);

        if (perfil == null)
        {
            return NotFound(new ApiResponse<UsuarioResponseDto>
            {
                Success = false,
                Message = "Usuario no encontrado."
            });
        }

        return Ok(new ApiResponse<UsuarioResponseDto>
        {
            Success = true,
            Message = "Perfil obtenido correctamente.",
            Data = perfil
        });
    }

    [HttpPut("perfil")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<object>>> ActualizarPerfil(
    ActualizarPerfilDto dto)
    {
        int? idUsuario =
            ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Token inválido."
            });
        }

        bool actualizado =
            await _usuarioService.ActualizarPerfilAsync(
                idUsuario.Value,
                dto);

        if (!actualizado)
        {
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Usuario no encontrado."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Perfil actualizado correctamente."
        });
    }

    [HttpPut("perfil/foto")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ApiResponse<string>>> ActualizarFotoPerfil(
    IFormFile foto)
    {
        int? idUsuario =
            ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized(new ApiResponse<string>
            {
                Success = false,
                Message = "Token inválido."
            });
        }

        if (foto == null || foto.Length == 0)
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Debe enviar una imagen."
            });
        }

        string[] extensionesPermitidas =
        {
        ".jpg",
        ".jpeg",
        ".png",
        ".bmp"
    };

        string extension =
            Path.GetExtension(foto.FileName).ToLower();

        if (!extensionesPermitidas.Contains(extension))
        {
            return BadRequest(new ApiResponse<string>
            {
                Success = false,
                Message = "Formato de imagen no permitido. Use JPG, JPEG, PNG o BMP."
            });
        }

        string webRootPath =
            _environment.WebRootPath ??
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot");

        string carpeta =
            Path.Combine(
                webRootPath,
                "uploads",
                "perfiles");

        if (!Directory.Exists(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }

        foreach (string archivoAnterior in Directory.GetFiles(
            carpeta,
            $"usuario_{idUsuario.Value}.*"))
        {
            System.IO.File.Delete(archivoAnterior);
        }

        string nombreArchivo =
            $"usuario_{idUsuario.Value}{extension}";

        string rutaArchivo =
            Path.Combine(carpeta, nombreArchivo);

        using (FileStream stream = new(
            rutaArchivo,
            FileMode.Create))
        {
            await foto.CopyToAsync(stream);
        }

        string fotoPerfilUrl =
            $"/uploads/perfiles/{nombreArchivo}";

        bool actualizado =
            await _usuarioService.ActualizarFotoPerfilAsync(
                idUsuario.Value,
                fotoPerfilUrl);

        if (!actualizado)
        {
            return NotFound(new ApiResponse<string>
            {
                Success = false,
                Message = "Usuario no encontrado."
            });
        }

        return Ok(new ApiResponse<string>
        {
            Success = true,
            Message = "Foto de perfil actualizada correctamente.",
            Data = fotoPerfilUrl
        });
    }

    [HttpPut("cambiar-contrasena")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<object>>> CambiarContrasena(
    CambiarContrasenaDto dto)
    {
        int? idUsuario =
            ObtenerIdUsuarioDesdeToken();

        if (idUsuario == null)
        {
            return Unauthorized(new ApiResponse<object>
            {
                Success = false,
                Message = "Token inválido."
            });
        }

        bool actualizada =
            await _usuarioService.CambiarContrasenaAsync(
                idUsuario.Value,
                dto);

        if (!actualizada)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "No se pudo cambiar la contraseña. Verifica la contraseña actual."
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Contraseña actualizada correctamente."
        });
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
