using SistemaHorarios.Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios
{
    public interface IUsuarioRepository
    {
        Task<Usuario> CrearAsync(Usuario usuario);

        Task<Usuario?> ObtenerPorIdAsync(int id);

        Task<IEnumerable<Usuario>> ObtenerTodosAsync();

        Task<bool> ActualizarAsync(Usuario usuario);

        Task<bool> EliminarAsync(Usuario usuario);

        Task<bool> ExisteCorreoAsync(string correo);

        Task<bool> ExisteCedulaAsync(string cedula);

        Task GuardarCambiosAsync();
    }
}