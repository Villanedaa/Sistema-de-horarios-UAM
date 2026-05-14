using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Materias
{
    // Gestiona las reglas de negocio relacionadas con prerrequisitos de materias.
    public class GestorPrerrequisito
    {
        private readonly MateriaRepository materiaRepository;
        private readonly PrerrequisitoRepository prerrequisitoRepository;
        private readonly ValidadorPrerrequisito validadorPrerrequisito;

        // Recibe los repositorios necesarios para trabajar con base de datos.
        public GestorPrerrequisito(
            MateriaRepository materiaRepository,
            PrerrequisitoRepository prerrequisitoRepository
        )
        {
            this.materiaRepository = materiaRepository;
            this.prerrequisitoRepository = prerrequisitoRepository;
            validadorPrerrequisito = new ValidadorPrerrequisito();
        }

        // Agrega un prerrequisito a una materia después de validar sus reglas.
        public async Task<List<string>> AgregarPrerrequisitoAsync(Prerrequisito prerrequisitoNuevo)
        {
            List<string> errores = await ValidarCreacionPrerrequisitoAsync(prerrequisitoNuevo);

            if (errores.Count > 0)
            {
                return errores;
            }

            PrepararPrerrequisitoNuevo(prerrequisitoNuevo);

            await prerrequisitoRepository.CrearPrerrequisitoAsync(prerrequisitoNuevo);

            return errores;
        }

        // Consulta un prerrequisito por su identificador.
        public async Task<Prerrequisito?> ConsultarPrerrequisitoPorIdAsync(int idPrerrequisito)
        {
            if (idPrerrequisito <= 0)
            {
                return null;
            }

            return await prerrequisitoRepository.ObtenerPrerrequisitoPorIdAsync(idPrerrequisito);
        }

        // Lista todos los prerrequisitos registrados.
        public async Task<List<Prerrequisito>> ListarPrerrequisitosAsync()
        {
            return await prerrequisitoRepository.ListarPrerrequisitosAsync();
        }

        // Lista los prerrequisitos activos asociados a una materia.
        public async Task<List<Prerrequisito>> ListarPrerrequisitosPorMateriaAsync(int idMateria)
        {
            if (idMateria <= 0)
            {
                return new List<Prerrequisito>();
            }

            return await prerrequisitoRepository.ListarPrerrequisitosPorMateriaAsync(idMateria);
        }

        // Desactiva un prerrequisito sin eliminarlo físicamente.
        public async Task<List<string>> DesactivarPrerrequisitoAsync(int idPrerrequisito)
        {
            List<string> errores = await ValidarDesactivacionPrerrequisitoAsync(idPrerrequisito);

            if (errores.Count > 0)
            {
                return errores;
            }

            await prerrequisitoRepository.DesactivarPrerrequisitoAsync(idPrerrequisito);

            return errores;
        }

        // Valida las reglas necesarias para agregar un prerrequisito.
        private async Task<List<string>> ValidarCreacionPrerrequisitoAsync(
            Prerrequisito prerrequisitoNuevo
        )
        {
            List<string> errores = validadorPrerrequisito.Validar(prerrequisitoNuevo);

            if (prerrequisitoNuevo == null)
            {
                return errores;
            }

            Materia? materiaPrincipal = await materiaRepository.ObtenerMateriaPorIdAsync(
                prerrequisitoNuevo.IdMateria
            );

            Materia? materiaPrerrequisito = await materiaRepository.ObtenerMateriaPorIdAsync(
                prerrequisitoNuevo.IdMateriaPrerrequisito
            );

            AgregarErrorSi(
                materiaPrincipal == null,
                errores,
                "La materia principal no existe."
            );

            AgregarErrorSi(
                materiaPrerrequisito == null,
                errores,
                "La materia prerrequisito no existe."
            );

            if (materiaPrincipal == null || materiaPrerrequisito == null)
            {
                return errores;
            }

            ValidarMateriasActivas(materiaPrincipal, materiaPrerrequisito, errores);
            await ValidarPrerrequisitoDuplicadoAsync(prerrequisitoNuevo, errores);
            await ValidarRelacionCircularDirectaAsync(prerrequisitoNuevo, errores);
            ValidarSemestrePrerrequisito(materiaPrincipal, materiaPrerrequisito, errores);

            return errores;
        }

        // Valida que ambas materias estén activas.
        private void ValidarMateriasActivas(
            Materia materiaPrincipal,
            Materia materiaPrerrequisito,
            List<string> errores
        )
        {
            AgregarErrorSi(
                !materiaPrincipal.Activa,
                errores,
                "La materia principal se encuentra inactiva."
            );

            AgregarErrorSi(
                !materiaPrerrequisito.Activa,
                errores,
                "La materia prerrequisito se encuentra inactiva."
            );
        }

        // Valida que no exista el mismo prerrequisito activo.
        private async Task ValidarPrerrequisitoDuplicadoAsync(
            Prerrequisito prerrequisitoNuevo,
            List<string> errores
        )
        {
            bool existePrerrequisito = await prerrequisitoRepository.ExistePrerrequisitoActivoAsync(
                prerrequisitoNuevo.IdMateria,
                prerrequisitoNuevo.IdMateriaPrerrequisito
            );

            AgregarErrorSi(
                existePrerrequisito,
                errores,
                "El prerrequisito ya está registrado para esta materia."
            );
        }

        // Valida que no exista una relación circular directa.
        private async Task ValidarRelacionCircularDirectaAsync(
            Prerrequisito prerrequisitoNuevo,
            List<string> errores
        )
        {
            bool existeRelacionCircular = await prerrequisitoRepository.ExisteRelacionCircularDirectaAsync(
                prerrequisitoNuevo.IdMateria,
                prerrequisitoNuevo.IdMateriaPrerrequisito
            );

            AgregarErrorSi(
                existeRelacionCircular,
                errores,
                "No se puede crear una relación circular directa entre materias."
            );
        }

        // Valida que el prerrequisito pertenezca a un semestre anterior.
        private void ValidarSemestrePrerrequisito(
            Materia materiaPrincipal,
            Materia materiaPrerrequisito,
            List<string> errores
        )
        {
            AgregarErrorSi(
                materiaPrerrequisito.Semestre >= materiaPrincipal.Semestre,
                errores,
                "La materia prerrequisito debe pertenecer a un semestre anterior."
            );
        }

        // Valida que el prerrequisito exista antes de desactivarlo.
        private async Task<List<string>> ValidarDesactivacionPrerrequisitoAsync(int idPrerrequisito)
        {
            List<string> errores = new List<string>();

            AgregarErrorSi(
                idPrerrequisito <= 0,
                errores,
                "El identificador del prerrequisito no es válido."
            );

            if (errores.Count > 0)
            {
                return errores;
            }

            Prerrequisito? prerrequisito = await prerrequisitoRepository.ObtenerPrerrequisitoPorIdAsync(
                idPrerrequisito
            );

            AgregarErrorSi(
                prerrequisito == null,
                errores,
                "El prerrequisito que desea desactivar no existe."
            );

            return errores;
        }

        // Prepara los valores iniciales de un prerrequisito nuevo.
        private void PrepararPrerrequisitoNuevo(Prerrequisito prerrequisitoNuevo)
        {
            prerrequisitoNuevo.Activo = true;
        }

        // Agrega un error cuando una condición no se cumple.
        private void AgregarErrorSi(bool condicion, List<string> errores, string mensaje)
        {
            if (condicion)
            {
                errores.Add(mensaje);
            }
        }
    }
}