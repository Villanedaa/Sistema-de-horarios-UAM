using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    [HttpGet]
    public IActionResult ObtenerUsuarios(
        [FromQuery] string? busqueda,
        [FromQuery] string? rol,
        [FromQuery] string? estado)
    {
        var usuarios = new[]
        {
            new
            {
                IdUsuario = 1,
                NombreCompleto = "Carlos Pérez",
                Cedula = "123456789",
                CorreoInstitucional = "carlos.perez@uam.edu.co",
                Rol = "Coordinador",
                Estado = "Activo"
            },
            new
            {
                IdUsuario = 2,
                NombreCompleto = "Ana Gómez",
                Cedula = "987654321",
                CorreoInstitucional = "ana.gomez@uam.edu.co",
                Rol = "Admin",
                Estado = "Activo"
            }
        };

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public IActionResult ObtenerUsuarioPorId(int id)
    {
        return Ok(new
        {
            IdUsuario = id,
            NombreCompleto = "Carlos Pérez",
            Cedula = "123456789",
            CorreoInstitucional = "carlos.perez@uam.edu.co",
            Rol = "Coordinador",
            Estado = "Activo"
        });
    }

    [HttpPost]
    public IActionResult CrearUsuario([FromBody] CrearUsuarioRequest request)
    {
        return Ok(new
        {
            IdUsuario = 3,
            request.NombreCompleto,
            request.Cedula,
            request.CorreoInstitucional,
            request.Rol,
            request.Estado,
            Mensaje = "Usuario creado correctamente."
        });
    }

    [HttpPut("{id}")]
    public IActionResult ActualizarUsuario(int id, [FromBody] ActualizarUsuarioRequest request)
    {
        return Ok(new
        {
            IdUsuario = id,
            request.NombreCompleto,
            request.Cedula,
            request.CorreoInstitucional,
            request.Rol,
            request.Estado,
            Mensaje = "Usuario actualizado correctamente."
        });
    }

    [HttpDelete("{id}")]
    public IActionResult EliminarUsuario(int id)
    {
        return Ok(new
        {
            IdUsuario = id,
            Mensaje = "Usuario inactivado correctamente."
        });
    }

    [HttpGet("perfil")]
    public IActionResult ObtenerPerfil()
    {
        return Ok(new
        {
            IdUsuario = 2,
            NombreCompleto = "Luis",
            CorreoInstitucional = "coordinador@uam.edu.co",
            Rol = "Coordinador",
            Telefono = "345678910",
            Programa = "Facultad de Ingeniería / Ingeniería de Sistemas"
        });
    }

    [HttpPut("perfil")]
    public IActionResult ActualizarPerfil([FromBody] ActualizarPerfilRequest request)
    {
        return Ok(new
        {
            request.NombreCompleto,
            request.Telefono,
            Mensaje = "Perfil actualizado correctamente."
        });
    }

    [HttpPut("cambiar-contrasena")]
    public IActionResult CambiarContrasena([FromBody] CambiarContrasenaRequest request)
    {
        return Ok(new
        {
            Mensaje = "Contraseña actualizada correctamente."
        });
    }

    [HttpGet("roles")]
    public IActionResult ObtenerRoles()
    {
        return Ok(new[]
        {
            "Admin",
            "Coordinador"
        });
    }

    [HttpGet("estados")]
    public IActionResult ObtenerEstados()
    {
        return Ok(new[]
        {
            "Activo",
            "Inactivo",
            "Pendiente"
        });
    }

    [HttpPost("verificar")]
    public IActionResult VerificarUsuario([FromBody] VerificarUsuarioRequest request)
    {
        return Ok(new
        {
            Existe = false,
            Mensaje = "El usuario puede ser registrado."
        });
    }
}

public class CrearUsuarioRequest
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string CorreoInstitucional { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class ActualizarUsuarioRequest
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string CorreoInstitucional { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public class ActualizarPerfilRequest
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
}

public class CambiarContrasenaRequest
{
    public string ContrasenaActual { get; set; } = string.Empty;
    public string NuevaContrasena { get; set; } = string.Empty;
    public string ConfirmarContrasena { get; set; } = string.Empty;
}

public class VerificarUsuarioRequest
{
    public string Cedula { get; set; } = string.Empty;
    public string CorreoInstitucional { get; set; } = string.Empty;
}