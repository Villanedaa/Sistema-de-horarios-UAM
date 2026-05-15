using Microsoft.AspNetCore.Mvc;
using SistemaHorarios.API.DTOs.Materias;
using SistemaHorarios.Logica.Negocio.Materias;
using SistemaHorarios.Modelos.DTOs.Materias;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.API.Controllers
{
    [ApiController]
    [Route("api/prerrequisitos")]
    public class PrerrequisitosController : ControllerBase
    {
        private readonly GestorPrerrequisito gestorPrerrequisito;
        private readonly GestorMateria gestorMateria;

        // Recibe los gestores necesarios para trabajar con prerrequisitos.
        public PrerrequisitosController(
            GestorPrerrequisito gestorPrerrequisito,
            GestorMateria gestorMateria)
        {
            this.gestorPrerrequisito = gestorPrerrequisito;
            this.gestorMateria = gestorMateria;
        }

        // Lista todos los prerrequisitos registrados.
        [HttpGet]
        public async Task<ActionResult<List<PrerrequisitoMateriaResponse>>> ObtenerPrerrequisitos()
        {
            List<Prerrequisito> prerrequisitos =
                await gestorPrerrequisito.ListarPrerrequisitosAsync();

            List<PrerrequisitoMateriaResponse> respuesta =
                new List<PrerrequisitoMateriaResponse>();

            foreach (Prerrequisito prerrequisito in prerrequisitos)
            {
                PrerrequisitoMateriaResponse item =
                    await MapearPrerrequisitoMateriaResponseAsync(prerrequisito);

                respuesta.Add(item);
            }

            return Ok(respuesta);
        }

        // Lista los prerrequisitos activos de una materia específica.
        [HttpGet("materia/{idMateria}")]
        public async Task<ActionResult<List<PrerrequisitoMateriaResponse>>> ObtenerPrerrequisitosPorMateria(
            int idMateria)
        {
            Materia? materia = await gestorMateria.ConsultarMateriaPorIdAsync(idMateria);

            if (materia == null)
            {
                return NotFound("La materia no existe.");
            }

            List<Prerrequisito> prerrequisitos =
                await gestorPrerrequisito.ListarPrerrequisitosPorMateriaAsync(idMateria);

            List<PrerrequisitoMateriaResponse> respuesta =
                new List<PrerrequisitoMateriaResponse>();

            foreach (Prerrequisito prerrequisito in prerrequisitos)
            {
                PrerrequisitoMateriaResponse item =
                    await MapearPrerrequisitoMateriaResponseAsync(prerrequisito);

                respuesta.Add(item);
            }

            return Ok(respuesta);
        }

        // Asigna un prerrequisito a una materia.
        [HttpPost]
        public async Task<IActionResult> CrearPrerrequisito(
            [FromBody] CrearPrerrequisitoRequest request)
        {
            Prerrequisito prerrequisito = new Prerrequisito
            {
                IdMateria = request.IdMateria,
                IdMateriaPrerrequisito = request.IdMateriaPrerrequisito
            };

            List<string> errores =
                await gestorPrerrequisito.AgregarPrerrequisitoAsync(prerrequisito);

            if (errores.Count > 0)
            {
                return BadRequest(errores);
            }

            PrerrequisitoMateriaResponse respuesta =
                await MapearPrerrequisitoMateriaResponseAsync(prerrequisito);

            return Ok(new
            {
                Mensaje = "Prerrequisito asignado correctamente.",
                Prerrequisito = respuesta
            });
        }

        // Desactiva un prerrequisito sin eliminarlo físicamente.
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPrerrequisito(int id)
        {
            List<string> errores =
                await gestorPrerrequisito.DesactivarPrerrequisitoAsync(id);

            if (errores.Count > 0)
            {
                return BadRequest(errores);
            }

            return Ok(new
            {
                IdPrerrequisito = id,
                Mensaje = "Prerrequisito desactivado correctamente."
            });
        }

        // Convierte un prerrequisito en respuesta para la API.
        private async Task<PrerrequisitoMateriaResponse> MapearPrerrequisitoMateriaResponseAsync(
            Prerrequisito prerrequisito)
        {
            Materia? materiaPrerrequisito =
                await gestorMateria.ConsultarMateriaPorIdAsync(
                    prerrequisito.IdMateriaPrerrequisito
                );

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
    }
}