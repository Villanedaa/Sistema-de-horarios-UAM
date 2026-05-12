using Microsoft.AspNetCore.Mvc;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Usuario == "admin@uam.edu.co")
        {
            return Ok(new
            {
                IdUsuario = 1,
                Nombre = "Administrador",
                Correo = request.Usuario,
                Rol = "Admin",
                Token = "token_admin_prueba"
            });
        }

        return Ok(new
        {
            IdUsuario = 2,
            Nombre = "Coordinador",
            Correo = request.Usuario,
            Rol = "Coordinador",
            Token = "token_coordinador_prueba"
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new
        {
            Mensaje = "Sesión cerrada correctamente."
        });
    }

    [HttpGet("perfil")]
    public IActionResult ObtenerPerfil()
    {
        return Ok(new
        {
            IdUsuario = 2,
            Nombre = "Coordinador",
            Correo = "coordinador@uam.edu.co",
            Rol = "Coordinador"
        });
    }
}

public class LoginRequest
{
    public string Usuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public bool RecordarSesion { get; set; }
}