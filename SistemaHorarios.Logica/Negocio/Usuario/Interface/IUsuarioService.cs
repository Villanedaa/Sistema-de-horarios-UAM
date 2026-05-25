using SistemaHorarios.Modelos.DTOs.Usuarios;

namespace SistemaHorarios.Logica.Negocio.Usuario.Interface;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> CrearUsuarioAsync(CrearUsuarioDto dto);

    Task<UsuarioResponseDto?> ObtenerPorIdAsync(int id);

    Task<IEnumerable<UsuarioResponseDto>> ObtenerTodosAsync();

    Task<IEnumerable<UsuarioResponseDto>> ObtenerCoordinadoresAsync();

    Task<bool> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto);

    Task<bool> CambiarEstadoUsuarioAsync(int id, string estado);

    Task<bool> EliminarUsuarioAsync(int id);

    Task<UsuarioResponseDto?> ObtenerPerfilAsync(int idUsuario);

    Task<bool> ActualizarPerfilAsync(int idUsuario, ActualizarPerfilDto dto);

    Task<bool> CambiarContrasenaAsync(int idUsuario, CambiarContrasenaDto dto);

    Task<VerificarUsuarioResponseDto> VerificarUsuarioAsync(VerificarUsuarioDto dto);

    Task<bool> ActualizarFotoPerfilAsync(int idUsuario, string fotoPerfilUrl);
}
