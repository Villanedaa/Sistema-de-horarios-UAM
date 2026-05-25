using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario> CrearAsync(Usuario usuario);

    Task<Usuario?> ObtenerPorIdAsync(int id);

    Task<IEnumerable<Usuario>> ObtenerTodosAsync();

    Task<bool> ActualizarAsync(Usuario usuario);

    Task<bool> EliminarAsync(Usuario usuario);

    Task<bool> ExisteCorreoAsync(string correo);

    Task<bool> ExisteCedulaAsync(string cedula);

    Task<bool> ExisteCorreoEnOtroUsuarioAsync(string correo, int idUsuarioActual);

    Task<bool> ExisteCedulaEnOtroUsuarioAsync(string cedula, int idUsuarioActual);

    Task GuardarCambiosAsync();
}
