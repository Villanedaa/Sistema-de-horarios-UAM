using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SistemaHorarios.Modelos.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaHorarios.Logica.Negocio.Auth;

/// <summary>
/// Servicio encargado de generar tokens JWT
/// para autenticación y autorización.
/// </summary>
public class JwtService
{
    private readonly IConfiguration _configuration;
    /// <summary>
    /// Inicializa una nueva instancia del servicio JWT.
    /// </summary>
    /// <param name="configuration">
    /// Configuración general de la aplicación.
    /// </param>
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// Genera un token JWT para el usuario autenticado.
    /// </summary>
    /// <param name="usuario">
    /// Usuario autenticado del sistema.
    /// </param>
    /// <returns>
    /// Token JWT serializado.
    /// </returns>

    public string GenerarToken(Usuario usuario)
    {
        var jwtSettings =
            _configuration.GetSection("Jwt");

        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.IdUsuario.ToString()),

            new Claim(
                ClaimTypes.Email,
                usuario.CorreoInstitucional),

            new Claim(
                ClaimTypes.Name,
                usuario.NombreCompleto),

            new Claim(
                ClaimTypes.Role,
                usuario.Rol.Nombre)
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                Convert.ToDouble(jwtSettings["ExpiresInMinutes"])
            ),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}