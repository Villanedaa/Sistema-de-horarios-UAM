using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.API.DTOs.Materias;
using SistemaHorarios.Logica.Negocio.Materias;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.API.Controllers
{
    [ApiController]
    [Route("api/materias")]
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
        public async Task<ActionResult<List<MateriaResumenResponse>>> ObtenerMaterias()
        {
            List<Materia> materias = await gestorMateria.ListarMateriasAsync();

            List<MateriaResumenResponse> respuesta = materias
                .Select(MapearMateriaResumen)
                .ToList();

            return Ok(respuesta);
        }

        // Lista únicamente las materias activas.
        [HttpGet("activas")]
        public async Task<ActionResult<List<MateriaActivaResponse>>> ObtenerMateriasActivas()
        {
            List<Materia> materias = await gestorMateria.ListarMateriasActivasAsync();

            List<MateriaActivaResponse> respuesta = materias
                .Select(MapearMateriaActiva)
                .ToList();

            return Ok(respuesta);
        }

        // Consulta una materia por su identificador.
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaResponse>> ObtenerMateriaPorId(int id)
        {
            Materia? materia = await gestorMateria.ConsultarMateriaPorIdAsync(id);

            if (materia == null)
            {
                return NotFound("La materia no existe.");
            }

            return Ok(MapearMateriaResponse(materia));
        }

        // Crea una nueva materia.
        [HttpPost]
        public async Task<IActionResult> CrearMateria([FromBody] CrearMateriaRequest request)
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
                return BadRequest(errores);
            }

            return Ok(new
            {
                Mensaje = "Materia creada correctamente.",
                Materia = MapearMateriaResponse(materia)
            });
        }

        // Actualiza una materia existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarMateria(
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
                return BadRequest(errores);
            }

            Materia? materiaActualizada = await gestorMateria.ConsultarMateriaPorIdAsync(id);

            if (materiaActualizada == null)
            {
                return NotFound("La materia no existe.");
            }

            return Ok(new
            {
                Mensaje = "Materia actualizada correctamente.",
                Materia = MapearMateriaResponse(materiaActualizada)
            });
        }

        // Desactiva una materia sin eliminarla físicamente.
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMateria(int id)
        {
            List<string> errores = await gestorMateria.DesactivarMateriaAsync(id);

            if (errores.Count > 0)
            {
                return BadRequest(errores);
            }

            return Ok(new
            {
                IdMateria = id,
                Mensaje = "Materia inactivada correctamente."
            });
        }

        // Lista los prerrequisitos activos asociados a una materia.
        [HttpGet("{id}/prerrequisitos")]
        public async Task<ActionResult<List<PrerrequisitoMateriaResponse>>> ObtenerPrerrequisitosDeMateria(int id)
        {
            Materia? materia = await gestorMateria.ConsultarMateriaPorIdAsync(id);

            if (materia == null)
            {
                return NotFound("La materia no existe.");
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

            return Ok(respuesta);
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