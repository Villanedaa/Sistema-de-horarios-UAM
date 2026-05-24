using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Grupos;
using SistemaHorarios.Modelos.DTOs.Grupos;
using SistemaHorarios.Modelos.Entidades;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/grupos")]
[Authorize(Roles = "Administrador,Coordinador")]
public class GruposController : ControllerBase
{
    private readonly GestorGrupo gestorGrupo;

    // Recibe el gestor de grupos para aplicar reglas de negocio.
    public GruposController(GestorGrupo gestorGrupo)
    {
        this.gestorGrupo = gestorGrupo;
    }

    // Lista todos los grupos registrados.
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<GrupoResumenResponse>>>> ObtenerGrupos()
    {
        List<Grupo> grupos = await gestorGrupo.ListarGruposAsync();

        List<GrupoResumenResponse> respuesta = grupos
            .Select(MapearGrupoResumen)
            .ToList();

        return Ok(new ApiResponse<List<GrupoResumenResponse>>
        {
            Success = true,
            Message = "Grupos consultados correctamente.",
            Data = respuesta
        });
    }

    // Consulta un grupo por su identificador.
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GrupoResponse>>> ObtenerGrupoPorId(int id)
    {
        Grupo? grupo = await gestorGrupo.ConsultarGrupoPorIdAsync(id);

        if (grupo == null)
        {
            return NotFound(new ApiResponse<GrupoResponse>
            {
                Success = false,
                Message = "El grupo no existe.",
                Data = null
            });
        }

        return Ok(new ApiResponse<GrupoResponse>
        {
            Success = true,
            Message = "Grupo consultado correctamente.",
            Data = MapearGrupoResponse(grupo)
        });
    }

    // Crea un nuevo grupo académico.
    [HttpPost]
    public async Task<ActionResult<ApiResponse<GrupoResponse>>> CrearGrupo(
        [FromBody] CrearGrupoRequest request)
    {
        Grupo grupo = new Grupo
        {
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            Jornada = request.Jornada,
            TipoGrupo = request.TipoGrupo,
            NumeroSemestre = request.NumeroSemestre,
            CantidadEstudiantes = request.CantidadEstudiantes,
            IdPlanAcademico = request.IdPlanAcademico,
            Materia = request.Materia,
            Dias = request.Dias
        };

        List<string> errores = await gestorGrupo.CrearGrupoAsync(grupo);

        if (errores.Count > 0)
        {
            return BadRequest(new ApiResponse<List<string>>
            {
                Success = false,
                Message = "No se pudo crear el grupo.",
                Data = errores
            });
        }

        return CreatedAtAction(
            nameof(ObtenerGrupoPorId),
            new { id = grupo.IdGrupo },
            new ApiResponse<GrupoResponse>
            {
                Success = true,
                Message = "Grupo creado correctamente.",
                Data = MapearGrupoResponse(grupo)
            }
        );
    }

    // Actualiza un grupo académico existente.
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<GrupoResponse>>> ActualizarGrupo(
        int id,
        [FromBody] ActualizarGrupoRequest request)
    {
        Grupo grupo = new Grupo
        {
            Codigo = request.Codigo,
            Nombre = request.Nombre,
            Jornada = request.Jornada,
            TipoGrupo = request.TipoGrupo,
            NumeroSemestre = request.NumeroSemestre,
            CantidadEstudiantes = request.CantidadEstudiantes,
            IdPlanAcademico = request.IdPlanAcademico,
            Materia = request.Materia,
            Dias = request.Dias,
            Activo = request.Activo
        };

        List<string> errores = await gestorGrupo.ModificarGrupoAsync(id, grupo);

        if (errores.Count > 0)
        {
            return BadRequest(new ApiResponse<List<string>>
            {
                Success = false,
                Message = "No se pudo actualizar el grupo.",
                Data = errores
            });
        }

        Grupo? grupoActualizado = await gestorGrupo.ConsultarGrupoPorIdAsync(id);

        if (grupoActualizado == null)
        {
            return NotFound(new ApiResponse<GrupoResponse>
            {
                Success = false,
                Message = "El grupo no existe.",
                Data = null
            });
        }

        return Ok(new ApiResponse<GrupoResponse>
        {
            Success = true,
            Message = "Grupo actualizado correctamente.",
            Data = MapearGrupoResponse(grupoActualizado)
        });
    }

    // Inactiva un grupo sin eliminarlo físicamente.
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<int>>> EliminarGrupo(int id)
    {
        List<string> errores = await gestorGrupo.DesactivarGrupoAsync(id);

        if (errores.Count > 0)
        {
            return BadRequest(new ApiResponse<List<string>>
            {
                Success = false,
                Message = "No se pudo inactivar el grupo.",
                Data = errores
            });
        }

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Grupo inactivado correctamente.",
            Data = id
        });
    }

// Activa nuevamente un grupo académico.
[HttpPatch("{id}/activar")]
public async Task<ActionResult<ApiResponse<int>>> ActivarGrupo(int id)
{
    List<string> errores = await gestorGrupo.ActivarGrupoAsync(id);

    if (errores.Count > 0)
    {
        return BadRequest(new ApiResponse<List<string>>
        {
            Success = false,
            Message = "No se pudo activar el grupo.",
            Data = errores
        });
    }

    return Ok(new ApiResponse<int>
    {
        Success = true,
        Message = "Grupo activado correctamente.",
        Data = id
    });
}

    // Consulta la cantidad de estudiantes asociada a un grupo.
    [HttpGet("{id}/cupos")]
    public async Task<ActionResult<ApiResponse<GrupoCuposResponse>>> ObtenerCuposGrupo(int id)
    {
        Grupo? grupo = await gestorGrupo.ConsultarGrupoPorIdAsync(id);

        if (grupo == null)
        {
            return NotFound(new ApiResponse<GrupoCuposResponse>
            {
                Success = false,
                Message = "El grupo no existe.",
                Data = null
            });
        }

        GrupoCuposResponse respuesta = new GrupoCuposResponse
        {
            IdGrupo = grupo.IdGrupo,
            CantidadEstudiantes = grupo.CantidadEstudiantes
        };

        return Ok(new ApiResponse<GrupoCuposResponse>
        {
            Success = true,
            Message = "Cupos del grupo consultados correctamente.",
            Data = respuesta
        });
    }

    // Lista únicamente los grupos activos.
    [HttpGet("activos")]
    public async Task<ActionResult<ApiResponse<List<GrupoActivoResponse>>>> ObtenerGruposActivos()
    {
        List<Grupo> grupos = await gestorGrupo.ListarGruposActivosAsync();

        List<GrupoActivoResponse> respuesta = grupos
            .Select(MapearGrupoActivo)
            .ToList();

        return Ok(new ApiResponse<List<GrupoActivoResponse>>
        {
            Success = true,
            Message = "Grupos activos consultados correctamente.",
            Data = respuesta
        });
    }

    // Lista los grupos asociados a un plan académico.
    [HttpGet("plan/{idPlanAcademico}")]
    public async Task<ActionResult<ApiResponse<List<GrupoResumenResponse>>>> ObtenerGruposPorPlanAcademico(
        int idPlanAcademico)
    {
        List<Grupo> grupos =
            await gestorGrupo.ListarGruposPorPlanAcademicoAsync(idPlanAcademico);

        List<GrupoResumenResponse> respuesta = grupos
            .Select(MapearGrupoResumen)
            .ToList();

        return Ok(new ApiResponse<List<GrupoResumenResponse>>
        {
            Success = true,
            Message = "Grupos del plan académico consultados correctamente.",
            Data = respuesta
        });
    }

    // Lista los grupos asociados a un número de semestre.
    [HttpGet("semestre/{numeroSemestre}")]
    public async Task<ActionResult<ApiResponse<List<GrupoResumenResponse>>>> ObtenerGruposPorSemestre(
        int numeroSemestre)
    {
        List<Grupo> grupos =
            await gestorGrupo.ListarGruposPorSemestreAsync(numeroSemestre);

        List<GrupoResumenResponse> respuesta = grupos
            .Select(MapearGrupoResumen)
            .ToList();

        return Ok(new ApiResponse<List<GrupoResumenResponse>>
        {
            Success = true,
            Message = "Grupos del semestre consultados correctamente.",
            Data = respuesta
        });
    }

    // Convierte una entidad Grupo en respuesta completa.
    private GrupoResponse MapearGrupoResponse(Grupo grupo)
    {
        return new GrupoResponse
        {
            IdGrupo = grupo.IdGrupo,
            Codigo = grupo.Codigo,
            Nombre = grupo.Nombre,
            Jornada = grupo.Jornada,
            TipoGrupo = grupo.TipoGrupo,
            NumeroSemestre = grupo.NumeroSemestre,
            CantidadEstudiantes = grupo.CantidadEstudiantes,
            IdPlanAcademico = grupo.IdPlanAcademico,
            Materia = grupo.Materia,
            Dias = grupo.Dias,
            Activo = grupo.Activo,
            EstadoTexto = ObtenerEstadoTexto(grupo.Activo)
        };
    }

    // Convierte una entidad Grupo en respuesta resumida.
    private GrupoResumenResponse MapearGrupoResumen(Grupo grupo)
    {
        return new GrupoResumenResponse
        {
            IdGrupo = grupo.IdGrupo,
            Codigo = grupo.Codigo,
            Nombre = grupo.Nombre,
            Jornada = grupo.Jornada,
            TipoGrupo = grupo.TipoGrupo,
            NumeroSemestre = grupo.NumeroSemestre,
            CantidadEstudiantes = grupo.CantidadEstudiantes,
            IdPlanAcademico = grupo.IdPlanAcademico,
            Materia = grupo.Materia,
            Dias = grupo.Dias,
            Activo = grupo.Activo,
            EstadoTexto = ObtenerEstadoTexto(grupo.Activo)
        };
    }

    // Convierte una entidad Grupo en respuesta para listas de selección.
    private GrupoActivoResponse MapearGrupoActivo(Grupo grupo)
    {
        return new GrupoActivoResponse
        {
            IdGrupo = grupo.IdGrupo,
            Codigo = grupo.Codigo,
            Nombre = grupo.Nombre
        };
    }

    // Convierte el estado booleano en texto legible.
    private string ObtenerEstadoTexto(bool activo)
    {
        if (activo)
        {
            return "Activo";
        }

        return "Inactivo";
    }
}