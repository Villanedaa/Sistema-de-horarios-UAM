using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SistemaHorarios.Logica.Negocio.Grupos;
using SistemaHorarios.Modelos.DTOs.Grupos;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Authorize(Roles = "Administrador,Coordinador")]
[Route("api/grupos")]
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
    public async Task<ActionResult<List<GrupoResumenResponse>>> ObtenerGrupos()
    {
        List<Grupo> grupos = await gestorGrupo.ListarGruposAsync();

        List<GrupoResumenResponse> respuesta = grupos
            .Select(MapearGrupoResumen)
            .ToList();

        return Ok(respuesta);
    }

    // Consulta un grupo por su identificador.
    [HttpGet("{id}")]
    public async Task<ActionResult<GrupoResponse>> ObtenerGrupoPorId(int id)
    {
        Grupo? grupo = await gestorGrupo.ConsultarGrupoPorIdAsync(id);

        if (grupo == null)
        {
            return NotFound("El grupo no existe.");
        }

        return Ok(MapearGrupoResponse(grupo));
    }

    // Crea un nuevo grupo académico.
    [HttpPost]
    public async Task<IActionResult> CrearGrupo([FromBody] CrearGrupoRequest request)
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
            return BadRequest(errores);
        }

        return Ok(new
        {
            Mensaje = "Grupo creado correctamente.",
            Grupo = MapearGrupoResponse(grupo)
        });
    }

    // Actualiza un grupo académico existente.
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarGrupo(
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
            return BadRequest(errores);
        }

        Grupo? grupoActualizado = await gestorGrupo.ConsultarGrupoPorIdAsync(id);

        if (grupoActualizado == null)
        {
            return NotFound("El grupo no existe.");
        }

        return Ok(new
        {
            Mensaje = "Grupo actualizado correctamente.",
            Grupo = MapearGrupoResponse(grupoActualizado)
        });
    }

    // Inactiva un grupo sin eliminarlo físicamente.
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarGrupo(int id)
    {
        List<string> errores = await gestorGrupo.DesactivarGrupoAsync(id);

        if (errores.Count > 0)
        {
            return BadRequest(errores);
        }

        return Ok(new
        {
            IdGrupo = id,
            Mensaje = "Grupo inactivado correctamente."
        });
    }

    // Consulta la cantidad de estudiantes asociada a un grupo.
    [HttpGet("{id}/cupos")]
    public async Task<ActionResult<GrupoCuposResponse>> ObtenerCuposGrupo(int id)
    {
        Grupo? grupo = await gestorGrupo.ConsultarGrupoPorIdAsync(id);

        if (grupo == null)
        {
            return NotFound("El grupo no existe.");
        }

        GrupoCuposResponse respuesta = new GrupoCuposResponse
        {
            IdGrupo = grupo.IdGrupo,
            CantidadEstudiantes = grupo.CantidadEstudiantes
        };

        return Ok(respuesta);
    }

    // Lista únicamente los grupos activos.
    [HttpGet("activos")]
    public async Task<ActionResult<List<GrupoActivoResponse>>> ObtenerGruposActivos()
    {
        List<Grupo> grupos = await gestorGrupo.ListarGruposActivosAsync();

        List<GrupoActivoResponse> respuesta = grupos
            .Select(MapearGrupoActivo)
            .ToList();

        return Ok(respuesta);
    }

    // Lista los grupos asociados a un plan académico.
    [HttpGet("plan/{idPlanAcademico}")]
    public async Task<ActionResult<List<GrupoResumenResponse>>> ObtenerGruposPorPlanAcademico(
        int idPlanAcademico)
    {
        List<Grupo> grupos =
            await gestorGrupo.ListarGruposPorPlanAcademicoAsync(idPlanAcademico);

        List<GrupoResumenResponse> respuesta = grupos
            .Select(MapearGrupoResumen)
            .ToList();

        return Ok(respuesta);
    }

    // Lista los grupos asociados a un número de semestre.
    [HttpGet("semestre/{numeroSemestre}")]
    public async Task<ActionResult<List<GrupoResumenResponse>>> ObtenerGruposPorSemestre(
        int numeroSemestre)
    {
        List<Grupo> grupos =
            await gestorGrupo.ListarGruposPorSemestreAsync(numeroSemestre);

        List<GrupoResumenResponse> respuesta = grupos
            .Select(MapearGrupoResumen)
            .ToList();

        return Ok(respuesta);
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
            Nombre = grupo.Nombre,
            NumeroSemestre = grupo.NumeroSemestre,
            IdPlanAcademico = grupo.IdPlanAcademico,
            Jornada = grupo.Jornada
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
