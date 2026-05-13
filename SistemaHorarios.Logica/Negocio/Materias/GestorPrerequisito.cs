using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Materias
{
    // Gestiona las operaciones principales relacionadas con los prerrequisitos de materias.
    public class GestorPrerrequisito
    {
        private readonly List<Prerrequisito> listaPrerrequisitos;
        private readonly ValidadorPrerrequisito validadorPrerrequisito;
        private readonly GestorMateria gestorMateria;

        // Inicializa el gestor con una lista temporal en memoria para pruebas.
        public GestorPrerrequisito(GestorMateria gestorMateria)
        {
            this.gestorMateria = gestorMateria;
            listaPrerrequisitos = new List<Prerrequisito>();
            validadorPrerrequisito = new ValidadorPrerrequisito();
        }

        // Agrega un prerrequisito a una materia.
        public List<string> AgregarPrerrequisito(Prerrequisito prerrequisitoNuevo)
        {
            List<string> errores = ValidarCreacionPrerrequisito(prerrequisitoNuevo);

            if (errores.Count > 0)
            {
                return errores;
            }

            PrepararPrerrequisitoNuevo(prerrequisitoNuevo);
            listaPrerrequisitos.Add(prerrequisitoNuevo);

            return errores;
        }

        // Consulta un prerrequisito por su identificador.
        public Prerrequisito? ConsultarPrerrequisitoPorId(int idPrerrequisito)
        {
            return listaPrerrequisitos.FirstOrDefault(
                prerrequisito => prerrequisito.IdPrerrequisito == idPrerrequisito
            );
        }

        // Lista todos los prerrequisitos registrados.
        public List<Prerrequisito> ListarPrerrequisitos()
        {
            return new List<Prerrequisito>(listaPrerrequisitos);
        }

        // Lista los prerrequisitos activos asociados a una materia.
        public List<Prerrequisito> ListarPrerrequisitosPorMateria(int idMateria)
        {
            if (idMateria <= 0)
            {
                return new List<Prerrequisito>();
            }

            return listaPrerrequisitos
                .Where(prerrequisito =>
                    prerrequisito.IdMateria == idMateria &&
                    prerrequisito.Activo
                )
                .ToList();
        }

        // Verifica si ya existe un prerrequisito activo entre dos materias.
        public bool ExistePrerrequisitoEntreMaterias(int idMateria, int idMateriaPrerrequisito)
        {
            return listaPrerrequisitos.Any(prerrequisito =>
                prerrequisito.IdMateria == idMateria &&
                prerrequisito.IdMateriaPrerrequisito == idMateriaPrerrequisito &&
                prerrequisito.Activo
            );
        }

        // Valida las reglas necesarias para agregar un prerrequisito.
        private List<string> ValidarCreacionPrerrequisito(Prerrequisito prerrequisitoNuevo)
        {
            List<string> errores = validadorPrerrequisito.Validar(prerrequisitoNuevo);

            if (prerrequisitoNuevo == null)
            {
                return errores;
            }

            AgregarErrorSi(
                !ExisteMateriaRegistrada(prerrequisitoNuevo.IdMateria),
                errores,
                "La materia principal no existe."
            );

            AgregarErrorSi(
                !ExisteMateriaRegistrada(prerrequisitoNuevo.IdMateriaPrerrequisito),
                errores,
                "La materia prerrequisito no existe."
            );

            AgregarErrorSi(
                ExistePrerrequisitoEntreMaterias(
                    prerrequisitoNuevo.IdMateria,
                    prerrequisitoNuevo.IdMateriaPrerrequisito
                ),
                errores,
                "El prerrequisito ya está registrado para esta materia."
            );

            AgregarErrorSi(
                ExisteRelacionCircularDirecta(
                    prerrequisitoNuevo.IdMateria,
                    prerrequisitoNuevo.IdMateriaPrerrequisito
                ),
                errores,
                "No se puede crear una relación circular directa entre materias."
            );

            return errores;
        }

        // Verifica si una materia existe en el gestor de materias.
        private bool ExisteMateriaRegistrada(int idMateria)
        {
            return gestorMateria.ConsultarMateriaPorId(idMateria) != null;
        }

        // Verifica si ya existe una relación inversa entre las mismas materias.
        private bool ExisteRelacionCircularDirecta(int idMateria, int idMateriaPrerrequisito)
        {
            return listaPrerrequisitos.Any(prerrequisito =>
                prerrequisito.IdMateria == idMateriaPrerrequisito &&
                prerrequisito.IdMateriaPrerrequisito == idMateria &&
                prerrequisito.Activo
            );
        }

        // Asigna los valores iniciales necesarios para un prerrequisito nuevo.
        private void PrepararPrerrequisitoNuevo(Prerrequisito prerrequisitoNuevo)
        {
            prerrequisitoNuevo.IdPrerrequisito = GenerarNuevoIdPrerrequisito();
            prerrequisitoNuevo.Activo = true;
        }

        // Genera un identificador temporal para pruebas en memoria.
        private int GenerarNuevoIdPrerrequisito()
        {
            if (listaPrerrequisitos.Count == 0)
            {
                return 1;
            }

            return listaPrerrequisitos.Max(prerrequisito => prerrequisito.IdPrerrequisito) + 1;
        }

        // Agrega un mensaje de error cuando una condición no se cumple.
        private void AgregarErrorSi(bool condicion, List<string> errores, string mensaje)
        {
            if (condicion)
            {
                errores.Add(mensaje);
            }
        }
    }
}