using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.Logica.Negocio.Horarios;
using SistemaHorarios.Modelos.DTOs.Horarios;
using SistemaHorarios.Modelos.Responses;
using HorarioEntidad = SistemaHorarios.Modelos.Entidades.Horario;

namespace SistemaHorarios.API.Controllers;

[ApiController]
[Route("api/horarios")]
[Authorize(Roles = "Administrador,Coordinador")]
public class HorariosController : ControllerBase
{
    private readonly GestorHorario gestorHorario;

    // Recibe el gestor de horarios para aplicar reglas de negocio.
    public HorariosController(GestorHorario gestorHorario)
    {
        this.gestorHorario = gestorHorario;
    }

    // Lista todos los horarios registrados.
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<HorarioResponse>>>> ObtenerHorarios()
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.ListarHorariosAsync();

        List<HorarioResponse> respuesta = horarios
            .Select(MapearHorarioResponse)
            .ToList();

        return Ok(new ApiResponse<List<HorarioResponse>>
        {
            Success = true,
            Message = "Horarios consultados correctamente.",
            Data = respuesta
        });
    }

    // Consulta un horario por su identificador.
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<HorarioResponse>>> ObtenerHorarioPorId(int id)
    {
        HorarioEntidad? horario =
            await gestorHorario.ConsultarHorarioPorIdAsync(id);

        if (horario == null)
        {
            return NotFound(new ApiResponse<HorarioResponse>
            {
                Success = false,
                Message = "El horario no existe.",
                Data = null
            });
        }

        return Ok(new ApiResponse<HorarioResponse>
        {
            Success = true,
            Message = "Horario consultado correctamente.",
            Data = MapearHorarioResponse(horario)
        });
    }

    // Crea una nueva asignación de horario.
    [HttpPost]
    public async Task<ActionResult<ApiResponse<HorarioResponse>>> CrearHorario(
        [FromBody] GenerarHorarioRequest request)
    {
        HorarioEntidad horario = new HorarioEntidad
        {
            IdGrupo = request.IdGrupo,
            IdMateria = request.IdMateria,
            IdDocente = request.IdDocente,
            IdFranjaHoraria = request.IdFranjaHoraria,
            Observacion = request.Observacion
        };

        List<string> errores =
            await gestorHorario.CrearHorarioAsync(horario);

        if (errores.Count > 0)
        {
            return BadRequest(new ApiResponse<List<string>>
            {
                Success = false,
                Message = "No se pudo crear el horario.",
                Data = errores
            });
        }

        HorarioEntidad? horarioCreado =
            await gestorHorario.ConsultarHorarioPorIdAsync(horario.IdHorario);

        HorarioResponse respuesta = horarioCreado == null
            ? MapearHorarioResponse(horario)
            : MapearHorarioResponse(horarioCreado);

        return CreatedAtAction(
            nameof(ObtenerHorarioPorId),
            new { id = horario.IdHorario },
            new ApiResponse<HorarioResponse>
            {
                Success = true,
                Message = "Horario creado correctamente.",
                Data = respuesta
            }
        );
    }

    // Actualiza una asignación de horario existente.
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<HorarioResponse>>> ActualizarHorario(
        int id,
        [FromBody] ActualizarHorarioRequest request)
    {
        HorarioEntidad horario = new HorarioEntidad
        {
            IdGrupo = request.IdGrupo,
            IdMateria = request.IdMateria,
            IdDocente = request.IdDocente,
            IdFranjaHoraria = request.IdFranjaHoraria,
            Observacion = request.Observacion,
            Activo = request.Activo
        };

        List<string> errores =
            await gestorHorario.ModificarHorarioAsync(id, horario);

        if (errores.Count > 0)
        {
            return BadRequest(new ApiResponse<List<string>>
            {
                Success = false,
                Message = "No se pudo actualizar el horario.",
                Data = errores
            });
        }

        HorarioEntidad? horarioActualizado =
            await gestorHorario.ConsultarHorarioPorIdAsync(id);

        if (horarioActualizado == null)
        {
            return NotFound(new ApiResponse<HorarioResponse>
            {
                Success = false,
                Message = "El horario no existe.",
                Data = null
            });
        }

        return Ok(new ApiResponse<HorarioResponse>
        {
            Success = true,
            Message = "Horario actualizado correctamente.",
            Data = MapearHorarioResponse(horarioActualizado)
        });
    }

    // Actualiza solo la materia, docente y franja de un horario.
    [HttpPut("{id}/asignatura")]
    public async Task<ActionResult<ApiResponse<HorarioResponse>>> ActualizarAsignaturaHorario(
        int id,
        [FromBody] ActualizarAsignaturaHorarioRequest request)
    {
        HorarioEntidad horarioActual =
            await gestorHorario.ConsultarHorarioPorIdAsync(id);

        if (horarioActual == null)
        {
            return NotFound(new ApiResponse<HorarioResponse>
            {
                Success = false,
                Message = "El horario no existe.",
                Data = null
            });
        }

        HorarioEntidad horario = new HorarioEntidad
        {
            IdGrupo = horarioActual.IdGrupo,
            IdMateria = request.IdMateria,
            IdDocente = request.IdDocente,
            IdFranjaHoraria = request.IdFranjaHoraria,
            Observacion = request.Observacion,
            Activo = horarioActual.Activo
        };

        List<string> errores =
            await gestorHorario.ModificarAsignaturaHorarioAsync(id, horario);

        if (errores.Count > 0)
        {
            return BadRequest(new ApiResponse<List<string>>
            {
                Success = false,
                Message = "No se pudo actualizar la asignatura del horario.",
                Data = errores
            });
        }

        HorarioEntidad? horarioActualizado =
            await gestorHorario.ConsultarHorarioPorIdAsync(id);

        return Ok(new ApiResponse<HorarioResponse>
        {
            Success = true,
            Message = "Asignatura del horario actualizada correctamente.",
            Data = horarioActualizado == null
                ? null
                : MapearHorarioResponse(horarioActualizado)
        });
    }

    // Inactiva un horario sin eliminarlo físicamente.
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<int>>> EliminarHorario(int id)
    {
        List<string> errores =
            await gestorHorario.DesactivarHorarioAsync(id);

        if (errores.Count > 0)
        {
            return BadRequest(new ApiResponse<List<string>>
            {
                Success = false,
                Message = "No se pudo inactivar el horario.",
                Data = errores
            });
        }

        return Ok(new ApiResponse<int>
        {
            Success = true,
            Message = "Horario inactivado correctamente.",
            Data = id
        });
    }

    // Genera automáticamente los bloques de horario para un grupo.
    [HttpPost("generar/{idGrupo}")]
    public async Task<ActionResult<ApiResponse<object>>> GenerarHorario(int idGrupo)
    {
        var (generados, errores) = await gestorHorario.GenerarHorariosAsync(idGrupo);

        if (errores.Count > 0 && generados == 0)
        {
            return BadRequest(new ApiResponse<List<string>>
            {
                Success = false,
                Message = "No se pudo generar el horario.",
                Data = errores
            });
        }

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Proceso de generación de horario finalizado.",
            Data = new
            {
                Generados = generados,
                Advertencias = errores
            }
        });
    }

    // Lista los horarios asociados a un grupo.
    [HttpGet("grupo/{idGrupo}")]
    public async Task<ActionResult<ApiResponse<List<HorarioResponse>>>> ObtenerHorariosPorGrupo(
        int idGrupo)
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.ListarHorariosPorGrupoAsync(idGrupo);

        List<HorarioResponse> respuesta = horarios
            .Select(MapearHorarioResponse)
            .ToList();

        return Ok(new ApiResponse<List<HorarioResponse>>
        {
            Success = true,
            Message = "Horarios del grupo consultados correctamente.",
            Data = respuesta
        });
    }

    // Lista los horarios asociados a un docente.
    [HttpGet("docente/{idDocente}")]
    public async Task<ActionResult<ApiResponse<List<HorarioDocenteResponse>>>> ObtenerHorariosPorDocente(
        int idDocente)
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.ListarHorariosPorDocenteAsync(idDocente);

        List<HorarioDocenteResponse> respuesta = horarios
            .Select(MapearHorarioDocenteResponse)
            .ToList();

        return Ok(new ApiResponse<List<HorarioDocenteResponse>>
        {
            Success = true,
            Message = "Horarios del docente consultados correctamente.",
            Data = respuesta
        });
    }

    // Lista los horarios asociados a una materia.
    [HttpGet("materia/{idMateria}")]
    public async Task<ActionResult<ApiResponse<List<HorarioResponse>>>> ObtenerHorariosPorMateria(
        int idMateria)
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.ListarHorariosPorMateriaAsync(idMateria);

        List<HorarioResponse> respuesta = horarios
            .Select(MapearHorarioResponse)
            .ToList();

        return Ok(new ApiResponse<List<HorarioResponse>>
        {
            Success = true,
            Message = "Horarios de la materia consultados correctamente.",
            Data = respuesta
        });
    }

    // Busca horarios por nombre, identificación o correo del docente.
    [HttpGet("docentes/buscar")]
    public async Task<ActionResult<ApiResponse<List<HorarioDocenteResponse>>>> BuscarHorariosPorDocente(
        [FromQuery] string busqueda)
    {
        List<HorarioEntidad> horarios =
            await gestorHorario.BuscarHorariosPorDocenteAsync(busqueda);

        List<HorarioDocenteResponse> respuesta = horarios
            .Select(MapearHorarioDocenteResponse)
            .ToList();

        return Ok(new ApiResponse<List<HorarioDocenteResponse>>
        {
            Success = true,
            Message = "Horarios encontrados correctamente.",
            Data = respuesta
        });
    }

    // Convierte una entidad Horario en respuesta completa.
    private HorarioResponse MapearHorarioResponse(HorarioEntidad horario)
    {
        TimeSpan horaInicio = horario.FranjaHoraria?.HoraInicio ?? TimeSpan.Zero;
        TimeSpan horaFin = horario.FranjaHoraria?.HoraFin ?? TimeSpan.Zero;

        return new HorarioResponse
        {
            IdHorario = horario.IdHorario,

            IdGrupo = horario.IdGrupo,
            CodigoGrupo = horario.Grupo?.Codigo ?? string.Empty,
            NombreGrupo = horario.Grupo?.Nombre ?? string.Empty,
            Jornada = horario.Grupo?.Jornada ?? string.Empty,
            TipoGrupo = horario.Grupo?.TipoGrupo ?? string.Empty,

            IdMateria = horario.IdMateria,
            CodigoMateria = horario.Materia?.Codigo ?? string.Empty,
            NombreMateria = horario.Materia?.Nombre ?? string.Empty,

            IdDocente = horario.IdDocente,
            NombreDocente = horario.Docente?.NombreCompleto ?? string.Empty,
            IdentificacionDocente = horario.Docente?.Identificacion ?? string.Empty,

            IdFranjaHoraria = horario.IdFranjaHoraria,
            DiaSemana = horario.FranjaHoraria?.DiaSemana ?? string.Empty,
            HoraInicio = horaInicio,
            HoraFin = horaFin,
            HorarioTexto = ObtenerHorarioTexto(horaInicio, horaFin),

            Observacion = horario.Observacion,
            Activo = horario.Activo,
            EstadoTexto = ObtenerEstadoTexto(horario.Activo)
        };
    }

    // Convierte una entidad Horario en respuesta para horario docente.
    private HorarioDocenteResponse MapearHorarioDocenteResponse(
        HorarioEntidad horario)
    {
        TimeSpan horaInicio = horario.FranjaHoraria?.HoraInicio ?? TimeSpan.Zero;
        TimeSpan horaFin = horario.FranjaHoraria?.HoraFin ?? TimeSpan.Zero;

        return new HorarioDocenteResponse
        {
            IdHorario = horario.IdHorario,

            IdDocente = horario.IdDocente,
            NombreDocente = horario.Docente?.NombreCompleto ?? string.Empty,
            IdentificacionDocente = horario.Docente?.Identificacion ?? string.Empty,
            CorreoInstitucional = horario.Docente?.CorreoInstitucional ?? string.Empty,

            IdGrupo = horario.IdGrupo,
            CodigoGrupo = horario.Grupo?.Codigo ?? string.Empty,
            NombreGrupo = horario.Grupo?.Nombre ?? string.Empty,

            IdMateria = horario.IdMateria,
            CodigoMateria = horario.Materia?.Codigo ?? string.Empty,
            NombreMateria = horario.Materia?.Nombre ?? string.Empty,

            DiaSemana = horario.FranjaHoraria?.DiaSemana ?? string.Empty,
            HoraInicio = horaInicio,
            HoraFin = horaFin,
            HorarioTexto = ObtenerHorarioTexto(horaInicio, horaFin),

            EstadoTexto = ObtenerEstadoTexto(horario.Activo)
        };
    }

    // Retorna el horario en formato legible.
    private string ObtenerHorarioTexto(TimeSpan horaInicio, TimeSpan horaFin)
    {
        return $"{horaInicio:hh\\:mm} - {horaFin:hh\\:mm}";
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