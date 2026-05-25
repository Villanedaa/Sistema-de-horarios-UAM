using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Logica.Negocio.Usuario.Interface;
using SistemaHorarios.Modelos.Constantes;
using SistemaHorarios.Modelos.DTOs.Usuarios;
using UsuarioEntidad = SistemaHorarios.Modelos.Entidades.Usuario;

namespace SistemaHorarios.Logica.Negocio.Usuario.Servicios;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly PasswordService _passwordService;

    public UsuarioService(
        IUsuarioRepository usuarioRepository,
        PasswordService passwordService)
    {
        _usuarioRepository = usuarioRepository;
        _passwordService = passwordService;
    }

    public async Task<UsuarioResponseDto> CrearUsuarioAsync(CrearUsuarioDto dto)
    {
        ValidarEstado(dto.Estado);

        bool existeCorreo =
            await _usuarioRepository.ExisteCorreoAsync(dto.CorreoInstitucional);

        if (existeCorreo)
        {
            throw new Exception("El correo ya está registrado.");
        }

        bool existeCedula =
            await _usuarioRepository.ExisteCedulaAsync(dto.Cedula);

        if (existeCedula)
        {
            throw new Exception("La cédula ya está registrada.");
        }

        var usuario = new UsuarioEntidad
        {
            NombreCompleto = dto.NombreCompleto.Trim(),
            Cedula = dto.Cedula.Trim(),
            CorreoInstitucional = dto.CorreoInstitucional.Trim(),
            ContrasenaHash = _passwordService.HashPassword(dto.Contrasena),
            IdRol = dto.IdRol,
            Estado = NormalizarEstado(dto.Estado),
            Celular = dto.Celular?.Trim() ?? string.Empty
        };

        var usuarioCreado = await _usuarioRepository.CrearAsync(usuario);

        var usuarioConRol =
            await _usuarioRepository.ObtenerPorIdAsync(usuarioCreado.IdUsuario);

        return MapearUsuario(usuarioConRol ?? usuarioCreado);
    }

    public async Task<UsuarioResponseDto?> ObtenerPorIdAsync(int id)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            return null;
        }

        return MapearUsuario(usuario);
    }

    public async Task<bool> ActualizarFotoPerfilAsync(
        int idUsuario,
        string fotoPerfilUrl)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
        {
            return false;
        }

        usuario.FotoPerfilUrl = fotoPerfilUrl;

        return await _usuarioRepository.ActualizarAsync(usuario);
    }

    public async Task<IEnumerable<UsuarioResponseDto>> ObtenerTodosAsync()
    {
        var usuarios = await _usuarioRepository.ObtenerTodosAsync();

        return usuarios.Select(MapearUsuario);
    }

    public async Task<IEnumerable<UsuarioResponseDto>> ObtenerCoordinadoresAsync()
    {
        var usuarios = await _usuarioRepository.ObtenerTodosAsync();

        return usuarios
            .Where(usuario =>
                string.Equals(
                    usuario.Rol?.Nombre,
                    RolesSistema.Coordinador,
                    StringComparison.OrdinalIgnoreCase))
            .Select(MapearUsuario);
    }

    public async Task<bool> ActualizarUsuarioAsync(
        int id,
        ActualizarUsuarioDto dto)
    {
        ValidarEstado(dto.Estado);

        var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            return false;
        }

        bool correoEnUso =
            await _usuarioRepository.ExisteCorreoEnOtroUsuarioAsync(
                dto.CorreoInstitucional,
                id);

        if (correoEnUso)
        {
            throw new Exception("El correo ya está registrado por otro usuario.");
        }

        bool cedulaEnUso =
            await _usuarioRepository.ExisteCedulaEnOtroUsuarioAsync(
                dto.Cedula,
                id);

        if (cedulaEnUso)
        {
            throw new Exception("La cédula ya está registrada por otro usuario.");
        }

        usuario.NombreCompleto = dto.NombreCompleto.Trim();
        usuario.Cedula = dto.Cedula.Trim();
        usuario.CorreoInstitucional = dto.CorreoInstitucional.Trim();
        usuario.IdRol = dto.IdRol;
        usuario.Estado = NormalizarEstado(dto.Estado);
        usuario.Celular = dto.Celular?.Trim() ?? string.Empty;

        return await _usuarioRepository.ActualizarAsync(usuario);
    }

    public async Task<bool> CambiarEstadoUsuarioAsync(
        int id,
        string estado)
    {
        ValidarEstado(estado);

        var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            return false;
        }

        usuario.Estado = NormalizarEstado(estado);

        return await _usuarioRepository.ActualizarAsync(usuario);
    }

    public async Task<bool> EliminarUsuarioAsync(int id)
    {
        // Para coordinadores y usuarios del sistema se usa inactivación lógica.
        return await CambiarEstadoUsuarioAsync(id, "Inactivo");
    }

    public async Task<UsuarioResponseDto?> ObtenerPerfilAsync(int idUsuario)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
        {
            return null;
        }

        return MapearUsuario(usuario);
    }

    public async Task<bool> ActualizarPerfilAsync(
        int idUsuario,
        ActualizarPerfilDto dto)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
        {
            return false;
        }

        usuario.NombreCompleto = dto.NombreCompleto.Trim();
        usuario.CorreoInstitucional = dto.CorreoInstitucional.Trim();
        usuario.Celular = dto.Celular?.Trim() ?? string.Empty;

        return await _usuarioRepository.ActualizarAsync(usuario);
    }

    public async Task<bool> CambiarContrasenaAsync(
        int idUsuario,
        CambiarContrasenaDto dto)
    {
        var usuario = await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
        {
            return false;
        }

        bool contrasenaActualValida =
            _passwordService.VerifyPassword(
                dto.ContrasenaActual,
                usuario.ContrasenaHash);

        if (!contrasenaActualValida)
        {
            return false;
        }

        usuario.ContrasenaHash =
            _passwordService.HashPassword(dto.NuevaContrasena);

        return await _usuarioRepository.ActualizarAsync(usuario);
    }

    public async Task<VerificarUsuarioResponseDto> VerificarUsuarioAsync(
        VerificarUsuarioDto dto)
    {
        bool existeCedula = false;
        bool existeCorreo = false;

        if (!string.IsNullOrWhiteSpace(dto.Cedula))
        {
            existeCedula =
                await _usuarioRepository.ExisteCedulaAsync(dto.Cedula);
        }

        if (!string.IsNullOrWhiteSpace(dto.CorreoInstitucional))
        {
            existeCorreo =
                await _usuarioRepository.ExisteCorreoAsync(
                    dto.CorreoInstitucional);
        }

        return new VerificarUsuarioResponseDto
        {
            ExisteCedula = existeCedula,
            ExisteCorreo = existeCorreo
        };
    }

    private static void ValidarEstado(string estado)
    {
        string estadoNormalizado = NormalizarEstado(estado);

        if (estadoNormalizado != "Activo" && estadoNormalizado != "Inactivo")
        {
            throw new Exception("El estado del usuario debe ser Activo o Inactivo.");
        }
    }

    private static string NormalizarEstado(string estado)
    {
        if (string.Equals(
            estado?.Trim(),
            "Inactivo",
            StringComparison.OrdinalIgnoreCase))
        {
            return "Inactivo";
        }

        return "Activo";
    }

    private static UsuarioResponseDto MapearUsuario(UsuarioEntidad usuario)
    {
        return new UsuarioResponseDto
        {
            IdUsuario = usuario.IdUsuario,
            NombreCompleto = usuario.NombreCompleto,
            Cedula = usuario.Cedula,
            CorreoInstitucional = usuario.CorreoInstitucional,
            IdRol = usuario.IdRol,
            Rol = usuario.Rol?.Nombre ?? string.Empty,
            Estado = usuario.Estado,
            Celular = usuario.Celular,
            FotoPerfilUrl = usuario.FotoPerfilUrl
        };
    }
}
