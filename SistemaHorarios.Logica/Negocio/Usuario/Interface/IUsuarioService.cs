using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaHorarios.Modelos.DTOs.Usuarios;

namespace SistemaHorarios.Logica.Negocio.Usuario.Interface;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> CrearUsuarioAsync(CrearUsuarioDto dto);

    Task<UsuarioResponseDto?> ObtenerPorIdAsync(int id);

    Task<IEnumerable<UsuarioResponseDto>> ObtenerTodosAsync();

    Task<bool> ActualizarUsuarioAsync(int id, ActualizarUsuarioDto dto);

    Task<bool> EliminarUsuarioAsync(int id);

    Task<UsuarioResponseDto?> ObtenerPerfilAsync(int idUsuario);

    Task<bool> ActualizarPerfilAsync(int idUsuario, ActualizarPerfilDto dto);

    Task<bool> CambiarContrasenaAsync(int idUsuario, CambiarContrasenaDto dto);

    Task<VerificarUsuarioResponseDto> VerificarUsuarioAsync(VerificarUsuarioDto dto);
}