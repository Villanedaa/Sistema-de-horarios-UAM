using SistemaHorarios.Modelos.DTOs.Roles;

namespace SistemaHorarios.Logica.Negocio.Roles.Interfaces;

public interface IRolService
{
    Task<List<RolResponseDto>> ObtenerRoles();

    Task<RolResponseDto?> ObtenerPorId(
        int idRol);

    Task CrearRol(
        CrearRolDto dto);

    Task ActualizarRol(
        int idRol,
        ActualizarRolDto dto);

    Task EliminarRol(
        int idRol);
}