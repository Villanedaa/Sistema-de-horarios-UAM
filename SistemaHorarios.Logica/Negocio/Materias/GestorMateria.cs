using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Materias
{
    // Gestiona las operaciones principales relacionadas con las materias.
    public class GestorMateria
    {
        private readonly List<Materia> listaMaterias;
        private readonly ValidadorMateria validadorMateria;

        // Inicializa el gestor con una lista temporal en memoria para pruebas.
        public GestorMateria()
        {
            listaMaterias = new List<Materia>();
            validadorMateria = new ValidadorMateria();
        }

        // Crea una nueva materia después de validar sus datos.
        public List<string> CrearMateria(Materia materiaNueva)
        {
            List<string> errores = ValidarCreacionMateria(materiaNueva);

            if (errores.Count > 0)
            {
                return errores;
            }

            PrepararMateriaNueva(materiaNueva);
            listaMaterias.Add(materiaNueva);

            return errores;
        }

        // Modifica la información de una materia existente.
        public List<string> ModificarMateria(int idMateria, Materia materiaModificada)
        {
            List<string> errores = ValidarModificacionMateria(idMateria, materiaModificada);

            if (errores.Count > 0)
            {
                return errores;
            }

            Materia? materiaActual = ConsultarMateriaPorId(idMateria);

            if (materiaActual == null)
            {
                errores.Add("La materia que desea modificar no existe.");
                return errores;
            }

            ActualizarDatosMateria(materiaActual, materiaModificada);

            return errores;
        }

        // Consulta una materia por su identificador.
        public Materia? ConsultarMateriaPorId(int idMateria)
        {
            return listaMaterias.FirstOrDefault(materia => materia.IdMateria == idMateria);
        }

        // Consulta una materia por su código.
        public Materia? ConsultarMateriaPorCodigo(string codigoMateria)
        {
            if (string.IsNullOrWhiteSpace(codigoMateria))
            {
                return null;
            }

            string codigoNormalizado = codigoMateria.Trim();

            return listaMaterias.FirstOrDefault(materia =>
                string.Equals(
                    materia.Codigo,
                    codigoNormalizado,
                    StringComparison.OrdinalIgnoreCase
                )
            );
        }

        // Lista todas las materias registradas.
        public List<Materia> ListarMaterias()
        {
            return new List<Materia>(listaMaterias);
        }

        // Lista únicamente las materias activas.
        public List<Materia> ListarMateriasActivas()
        {
            return listaMaterias
                .Where(materia => materia.Activa)
                .ToList();
        }

        // Valida las reglas necesarias para crear una materia.
        private List<string> ValidarCreacionMateria(Materia materiaNueva)
        {
            List<string> errores = validadorMateria.Validar(materiaNueva);

            if (materiaNueva == null)
            {
                return errores;
            }

            AgregarErrorSi(
                ExisteMateriaConCodigo(materiaNueva.Codigo),
                errores,
                "Ya existe una materia registrada con el mismo código."
            );

            return errores;
        }

        // Valida las reglas necesarias para modificar una materia.
        private List<string> ValidarModificacionMateria(int idMateria, Materia materiaModificada)
        {
            List<string> errores = validadorMateria.Validar(materiaModificada);

            AgregarErrorSi(
                idMateria <= 0,
                errores,
                "El identificador de la materia no es válido."
            );

            if (materiaModificada == null)
            {
                return errores;
            }

            AgregarErrorSi(
                ConsultarMateriaPorId(idMateria) == null,
                errores,
                "La materia que desea modificar no existe."
            );

            AgregarErrorSi(
                ExisteOtraMateriaConMismoCodigo(materiaModificada.Codigo, idMateria),
                errores,
                "Ya existe otra materia registrada con el mismo código."
            );

            return errores;
        }

        // Asigna los valores iniciales necesarios para una materia nueva.
        private void PrepararMateriaNueva(Materia materiaNueva)
        {
            materiaNueva.IdMateria = GenerarNuevoIdMateria();
            materiaNueva.Codigo = materiaNueva.Codigo.Trim();
            materiaNueva.Nombre = materiaNueva.Nombre.Trim();
            materiaNueva.Activa = true;
        }

        // Actualiza los datos modificables de una materia existente.
        private void ActualizarDatosMateria(Materia materiaActual, Materia materiaModificada)
        {
            materiaActual.Codigo = materiaModificada.Codigo.Trim();
            materiaActual.Nombre = materiaModificada.Nombre.Trim();
            materiaActual.Creditos = materiaModificada.Creditos;
            materiaActual.IntensidadHorariaSemanal = materiaModificada.IntensidadHorariaSemanal;
            materiaActual.Semestre = materiaModificada.Semestre;
            materiaActual.Activa = materiaModificada.Activa;
        }

        // Verifica si ya existe una materia con el código indicado.
        private bool ExisteMateriaConCodigo(string codigoMateria)
        {
            if (string.IsNullOrWhiteSpace(codigoMateria))
            {
                return false;
            }

            string codigoNormalizado = codigoMateria.Trim();

            return listaMaterias.Any(materia =>
                string.Equals(
                    materia.Codigo,
                    codigoNormalizado,
                    StringComparison.OrdinalIgnoreCase
                )
            );
        }

        // Verifica si otra materia diferente ya tiene el mismo código.
        private bool ExisteOtraMateriaConMismoCodigo(string codigoMateria, int idMateriaActual)
        {
            if (string.IsNullOrWhiteSpace(codigoMateria))
            {
                return false;
            }

            string codigoNormalizado = codigoMateria.Trim();

            return listaMaterias.Any(materia =>
                materia.IdMateria != idMateriaActual &&
                string.Equals(
                    materia.Codigo,
                    codigoNormalizado,
                    StringComparison.OrdinalIgnoreCase
                )
            );
        }

        // Genera un identificador temporal para pruebas en memoria.
        private int GenerarNuevoIdMateria()
        {
            if (listaMaterias.Count == 0)
            {
                return 1;
            }

            return listaMaterias.Max(materia => materia.IdMateria) + 1;
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