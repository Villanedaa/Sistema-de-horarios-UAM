using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.API.DTOs.Materias;
using SistemaHorarios.Logica.Negocio.Materias;
using SistemaHorarios.Modelos.Entidades;
using SistemaHorarios.Modelos.Responses;

namespace SistemaHorarios.API.Controllers
{
    [ApiController]
    [Route("api/materias")]
    [Authorize(Roles = "Administrador,Coordinador")]
    public class MateriasController : ControllerBase
    {
        private readonly GestorMateria gestorMateria;
        private readonly GestorPrerrequisito gestorPrerrequisito;

        // Recibe los gestores necesarios para trabajar con materias y prerrequisitos.
        public MateriasController(
            GestorMateria gestorMateria,
            GestorPrerrequisito gestorPrerrequisito)
        {
            this.gestorMateria = gestorMateria;
            this.gestorPrerrequisito = gestorPrerrequisito;
        }

        // Lista todas las materias registradas.
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<MateriaResumenResponse>>>> ObtenerMaterias()
        {
            List<Materia> materias = await gestorMateria.ListarMateriasAsync();

            List<MateriaResumenResponse> respuesta = materias
                .Select(MapearMateriaResumen)
                .ToList();

            return Ok(new ApiResponse<List<MateriaResumenResponse>>
            {
                Success = true,
                Message = "Materias consultadas correctamente.",
                Data = respuesta
            });
        }

        // Lista únicamente las materias activas.
        [HttpGet("activas")]
        public async Task<ActionResult<ApiResponse<List<MateriaActivaResponse>>>> ObtenerMateriasActivas()
        {
            List<Materia> materias = await gestorMateria.ListarMateriasActivasAsync();

            List<MateriaActivaResponse> respuesta = materias
                .Select(MapearMateriaActiva)
                .ToList();

            return Ok(new ApiResponse<List<MateriaActivaResponse>>
            {
                Success = true,
                Message = "Materias activas consultadas correctamente.",
                Data = respuesta
            });
        }

        // Consulta una materia por su identificador.
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MateriaResponse>>> ObtenerMateriaPorId(int id)
        {
            Materia? materia = await gestorMateria.ConsultarMateriaPorIdAsync(id);

            if (materia == null)
            {
                return NotFound(new ApiResponse<MateriaResponse>
                {
                    Success = false,
                    Message = "La materia no existe.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<MateriaResponse>
            {
                Success = true,
                Message = "Materia consultada correctamente.",
                Data = MapearMateriaResponse(materia)
            });
        }

        // Crea una nueva materia.
        [HttpPost]
        public async Task<ActionResult<ApiResponse<MateriaResponse>>> CrearMateria(
            [FromBody] CrearMateriaRequest request)
        {
            Materia materia = new Materia
            {
                Codigo = request.Codigo,
                Nombre = request.Nombre,
                Creditos = request.Creditos,
                IntensidadHorariaSemanal = request.IntensidadHorariaSemanal,
                Semestre = request.Semestre,
                CantidadGrupos = request.CantidadGrupos
            };

            List<string> errores = await gestorMateria.CrearMateriaAsync(materia);

            if (errores.Count > 0)
            {
                return BadRequest(new ApiResponse<List<string>>
                {
                    Success = false,
                    Message = "No se pudo crear la materia.",
                    Data = errores
                });
            }

            return CreatedAtAction(
                nameof(ObtenerMateriaPorId),
                new { id = materia.IdMateria },
                new ApiResponse<MateriaResponse>
                {
                    Success = true,
                    Message = "Materia creada correctamente.",
                    Data = MapearMateriaResponse(materia)
                }
            );
        }

        // Actualiza una materia existente.
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<MateriaResponse>>> ActualizarMateria(
            int id,
            [FromBody] ActualizarMateriaRequest request)
        {
            Materia materia = new Materia
            {
                Codigo = request.Codigo,
                Nombre = request.Nombre,
                Creditos = request.Creditos,
                IntensidadHorariaSemanal = request.IntensidadHorariaSemanal,
                Semestre = request.Semestre,
                CantidadGrupos = request.CantidadGrupos,
                Activa = request.Activa
            };

            List<string> errores = await gestorMateria.ModificarMateriaAsync(id, materia);

            if (errores.Count > 0)
            {
                return BadRequest(new ApiResponse<List<string>>
                {
                    Success = false,
                    Message = "No se pudo actualizar la materia.",
                    Data = errores
                });
            }

            Materia? materiaActualizada = await gestorMateria.ConsultarMateriaPorIdAsync(id);

            if (materiaActualizada == null)
            {
                return NotFound(new ApiResponse<MateriaResponse>
                {
                    Success = false,
                    Message = "La materia no existe.",
                    Data = null
                });
            }

            return Ok(new ApiResponse<MateriaResponse>
            {
                Success = true,
                Message = "Materia actualizada correctamente.",
                Data = MapearMateriaResponse(materiaActualizada)
            });
        }

        // Desactiva una materia sin eliminarla físicamente.
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<int>>> EliminarMateria(int id)
        {
            List<string> errores = await gestorMateria.DesactivarMateriaAsync(id);

            if (errores.Count > 0)
            {
                return BadRequest(new ApiResponse<List<string>>
                {
                    Success = false,
                    Message = "No se pudo inactivar la materia.",
                    Data = errores
                });
            }

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Materia inactivada correctamente.",
                Data = id
            });
        }



        // Reactiva una materia previamente inactiva.
        [HttpPatch("{id}/activar")]
        public async Task<ActionResult<ApiResponse<int>>> ActivarMateria(int id)
        {
            List<string> errores = await gestorMateria.ActivarMateriaAsync(id);

            if (errores.Count > 0)
            {
                return BadRequest(new ApiResponse<List<string>>
                {
                    Success = false,
                    Message = "No se pudo activar la materia.",
                    Data = errores
                });
            }

            return Ok(new ApiResponse<int>
            {
                Success = true,
                Message = "Materia activada correctamente.",
                Data = id
            });
        }

        // Lista los prerrequisitos activos asociados a una materia.
        [HttpGet("{id}/prerrequisitos")]
        public async Task<ActionResult<ApiResponse<List<PrerrequisitoMateriaResponse>>>> ObtenerPrerrequisitosDeMateria(
            int id)
        {
            Materia? materia = await gestorMateria.ConsultarMateriaPorIdAsync(id);

            if (materia == null)
            {
                return NotFound(new ApiResponse<List<PrerrequisitoMateriaResponse>>
                {
                    Success = false,
                    Message = "La materia no existe.",
                    Data = null
                });
            }

            List<Prerrequisito> prerrequisitos =
                await gestorPrerrequisito.ListarPrerrequisitosPorMateriaAsync(id);

            List<PrerrequisitoMateriaResponse> respuesta = new List<PrerrequisitoMateriaResponse>();

            foreach (Prerrequisito prerrequisito in prerrequisitos)
            {
                PrerrequisitoMateriaResponse item =
                    await MapearPrerrequisitoMateriaResponseAsync(prerrequisito);

                respuesta.Add(item);
            }

            return Ok(new ApiResponse<List<PrerrequisitoMateriaResponse>>
            {
                Success = true,
                Message = "Prerrequisitos de la materia consultados correctamente.",
                Data = respuesta
            });
        }

        // Convierte una entidad Materia en respuesta completa.
        private MateriaResponse MapearMateriaResponse(Materia materia)
        {
            return new MateriaResponse
            {
                IdMateria = materia.IdMateria,
                Codigo = materia.Codigo,
                Nombre = materia.Nombre,
                Creditos = materia.Creditos,
                IntensidadHorariaSemanal = materia.IntensidadHorariaSemanal,
                Semestre = materia.Semestre,
                CantidadGrupos = materia.CantidadGrupos,
                Activa = materia.Activa,
                EstadoTexto = ObtenerEstadoTexto(materia.Activa)
            };
        }

        // Convierte una entidad Materia en respuesta resumida.
        private MateriaResumenResponse MapearMateriaResumen(Materia materia)
        {
            return new MateriaResumenResponse
            {
                IdMateria = materia.IdMateria,
                Codigo = materia.Codigo,
                Nombre = materia.Nombre,
                Creditos = materia.Creditos,
                IntensidadHorariaSemanal = materia.IntensidadHorariaSemanal,
                Semestre = materia.Semestre,
                CantidadGrupos = materia.CantidadGrupos,
                Activa = materia.Activa,
                EstadoTexto = ObtenerEstadoTexto(materia.Activa)
            };
        }

        // Convierte una entidad Materia en respuesta para listas de selección.
        private MateriaActivaResponse MapearMateriaActiva(Materia materia)
        {
            return new MateriaActivaResponse
            {
                IdMateria = materia.IdMateria,
                Codigo = materia.Codigo,
                Nombre = materia.Nombre
            };
        }

        // Convierte un prerrequisito en respuesta para la API.
        private async Task<PrerrequisitoMateriaResponse> MapearPrerrequisitoMateriaResponseAsync(
            Prerrequisito prerrequisito)
        {
            Materia? materiaPrerrequisito =
                await gestorMateria.ConsultarMateriaPorIdAsync(prerrequisito.IdMateriaPrerrequisito);

            return new PrerrequisitoMateriaResponse
            {
                IdPrerrequisito = prerrequisito.IdPrerrequisito,
                IdMateria = prerrequisito.IdMateria,
                IdMateriaPrerrequisito = prerrequisito.IdMateriaPrerrequisito,
                CodigoMateriaPrerrequisito = materiaPrerrequisito?.Codigo ?? string.Empty,
                NombreMateriaPrerrequisito = materiaPrerrequisito?.Nombre ?? string.Empty,
                Activo = prerrequisito.Activo
            };
        }

        // Convierte el estado booleano en texto legible.
        private string ObtenerEstadoTexto(bool activa)
        {
            if (activa)
            {
                return "Activa";
            }

            return "Inactiva";
        }
    }
}