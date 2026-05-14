using SistemaHorarios.Modelos.DTOs.Auth;

namespace SistemaHorarios.Logica.Negocio.Auth;

/// <summary>
/// Contrato encargado de definir las operaciones
/// de autenticación y autorización de usuarios.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Valida credenciales de usuario y genera JWT.
    /// </summary>
    /// <param name="dto">
    /// Credenciales usuario.
    /// </param>
    /// <returns>
    /// Información autenticación y token JWT.
    /// </returns>
    Task<LoginResponseDto> Login(
        LoginRequestDto dto);

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="dto">
    /// Información usuario registrar.
    /// </param>
    Task Registrar(
        RegistroUsuarioDto dto);

    /// <summary>
    /// Obtiene información del usuario autenticado.
    /// </summary>
    /// <param name="idUsuario">
    /// Identificador usuario.
    /// </param>
    /// <returns>
    /// Información perfil usuario.
    /// </returns>
    Task<PerfilUsuarioDto> ObtenerPerfil(
        int idUsuario);
}