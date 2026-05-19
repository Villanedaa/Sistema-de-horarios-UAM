using SistemaHorarios.Datos.Interfaces;
using SistemaHorarios.Logica.Negocio.Auth;
using SistemaHorarios.Logica.Negocio.Usuario.Interface;
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

    public async Task<UsuarioResponseDto> CrearUsuarioAsync(
        CrearUsuarioDto dto)
    {
        bool existeCorreo =
            await _usuarioRepository.ExisteCorreoAsync(
                dto.CorreoInstitucional);

        if (existeCorreo)
        {
            throw new Exception("El correo ya está registrado.");
        }

        bool existeCedula =
            await _usuarioRepository.ExisteCedulaAsync(
                dto.Cedula);

        if (existeCedula)
        {
            throw new Exception("La cédula ya está registrada.");
        }

        var usuario = new UsuarioEntidad
        {
            NombreCompleto = dto.NombreCompleto,
            Cedula = dto.Cedula,
            CorreoInstitucional = dto.CorreoInstitucional,
            ContrasenaHash =
                _passwordService.HashPassword(dto.Contrasena),
            IdRol = dto.IdRol,
            Estado = dto.Estado,
            Celular = dto.Celular
        };

        var usuarioCreado =
            await _usuarioRepository.CrearAsync(usuario);

        var usuarioConRol =
            await _usuarioRepository.ObtenerPorIdAsync(
                usuarioCreado.IdUsuario);

        if (usuarioConRol == null)
        {
            return MapearUsuario(usuarioCreado);
        }

        return MapearUsuario(usuarioConRol);
    }

    public async Task<UsuarioResponseDto?> ObtenerPorIdAsync(int id)
    {
        var usuario =
            await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            return null;
        }

        return MapearUsuario(usuario);
    }

    public async Task<IEnumerable<UsuarioResponseDto>> ObtenerTodosAsync()
    {
        var usuarios =
            await _usuarioRepository.ObtenerTodosAsync();

        return usuarios.Select(usuario => MapearUsuario(usuario));
    }

    public async Task<bool> ActualizarUsuarioAsync(
        int id,
        ActualizarUsuarioDto dto)
    {
        var usuario =
            await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            return false;
        }

        usuario.NombreCompleto = dto.NombreCompleto;
        usuario.Cedula = dto.Cedula;
        usuario.CorreoInstitucional = dto.CorreoInstitucional;
        usuario.IdRol = dto.IdRol;
        usuario.Estado = dto.Estado;
        usuario.Celular = dto.Celular;

        return await _usuarioRepository.ActualizarAsync(usuario);
    }

    public async Task<bool> EliminarUsuarioAsync(int id)
    {
        var usuario =
            await _usuarioRepository.ObtenerPorIdAsync(id);

        if (usuario == null)
        {
            return false;
        }

        return await _usuarioRepository.EliminarAsync(usuario);
    }

    public async Task<UsuarioResponseDto?> ObtenerPerfilAsync(
        int idUsuario)
    {
        var usuario =
            await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

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
        var usuario =
            await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

        if (usuario == null)
        {
            return false;
        }

        usuario.NombreCompleto = dto.NombreCompleto;
        usuario.CorreoInstitucional = dto.CorreoInstitucional;

        return await _usuarioRepository.ActualizarAsync(usuario);
    }

    public async Task<bool> CambiarContrasenaAsync(
        int idUsuario,
        CambiarContrasenaDto dto)
    {
        var usuario =
            await _usuarioRepository.ObtenerPorIdAsync(idUsuario);

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
            _passwordService.HashPassword(
                dto.NuevaContrasena);

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
                await _usuarioRepository.ExisteCedulaAsync(
                    dto.Cedula);
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

    private UsuarioResponseDto MapearUsuario(
        UsuarioEntidad usuario)
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
            Celular = usuario.Celular
        };
    }
}