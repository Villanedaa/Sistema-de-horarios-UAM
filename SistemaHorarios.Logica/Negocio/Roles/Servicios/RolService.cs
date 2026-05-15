using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Logica.Negocio.Roles.Interfaces;
using SistemaHorarios.Modelos.DTOs.Roles;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Roles.Servicios;

public class RolService : IRolService
{
    private readonly RolRepository _rolRepository;

    public RolService(
        RolRepository rolRepository)
    {
        _rolRepository = rolRepository;
    }

    // Obtiene todos los roles
    public async Task<List<RolResponseDto>>
        ObtenerRoles()
    {
        var roles =
            await _rolRepository.ObtenerRoles();

        return roles.Select(r => new RolResponseDto
        {
            IdRol = r.IdRol,

            Nombre = r.Nombre,

            Descripcion = r.Descripcion,

            Activo = r.Activo
        }).ToList();
    }

    // Obtiene un rol por Id
    public async Task<RolResponseDto?>
        ObtenerPorId(
            int idRol)
    {
        var rol =
            await _rolRepository
                .ObtenerPorId(idRol);

        if (rol == null)
        {
            return null;
        }

        return new RolResponseDto
        {
            IdRol = rol.IdRol,

            Nombre = rol.Nombre,

            Descripcion = rol.Descripcion,

            Activo = rol.Activo
        };
    }

    // Crea un nuevo rol
    public async Task CrearRol(
        CrearRolDto dto)
    {
        var rolExistente =
            await _rolRepository
                .ObtenerPorNombre(dto.Nombre);

        if (rolExistente != null)
        {
            throw new Exception(
                "El rol ya existe"
            );
        }

        var rol = new Rol
        {
            Nombre = dto.Nombre,

            Descripcion = dto.Descripcion
        };

        await _rolRepository.CrearRol(rol);
    }

    // Actualiza un rol existente
    public async Task ActualizarRol(
        int idRol,
        ActualizarRolDto dto)
    {
        var rol =
            await _rolRepository
                .ObtenerPorId(idRol);

        if (rol == null)
        {
            throw new Exception(
                "Rol no encontrado"
            );
        }

        rol.Nombre = dto.Nombre;

        rol.Descripcion = dto.Descripcion;

        rol.Activo = dto.Activo;

        await _rolRepository.ActualizarRol(rol);
    }

    // Elimina un rol
    public async Task EliminarRol(
        int idRol)
    {
        var rol =
            await _rolRepository
                .ObtenerPorId(idRol);

        if (rol == null)
        {
            throw new Exception(
                "Rol no encontrado"
            );
        }

        await _rolRepository.EliminarRol(rol);
    }
}