using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Materias
{
    // Gestiona las reglas de negocio relacionadas con las materias.
    public class GestorMateria
    {
       
        private readonly MateriaRepository materiaRepositorio;
        private readonly ValidadorMateria validadorMateria;

        // Recibe el repositorio de materias para trabajar con base de datos.
        public GestorMateria(MateriaRepository materiaRepositorio)
        {
            this.materiaRepositorio = materiaRepositorio;
            validadorMateria = new ValidadorMateria();
        }

        // Crea una nueva materia después de validar sus datos.
        public async Task<List<string>> CrearMateriaAsync(Materia materiaNueva)
        {
            List<string> errores = await ValidarCreacionMateriaAsync(materiaNueva);

            if (errores.Count > 0)
            {
                return errores;
            }

            PrepararMateriaNueva(materiaNueva);

            await materiaRepositorio.CrearMateriaAsync(materiaNueva);

            return errores;
        }

        // Modifica la información de una materia existente.
        public async Task<List<string>> ModificarMateriaAsync(int idMateria, Materia materiaModificada)
        {
            List<string> errores = await ValidarModificacionMateriaAsync(idMateria, materiaModificada);

            if (errores.Count > 0)
            {
                return errores;
            }

            PrepararMateriaModificada(idMateria, materiaModificada);

            await materiaRepositorio.ActualizarMateriaAsync(materiaModificada);

            return errores;
        }

        // Consulta una materia por su identificador.
        public async Task<Materia?> ConsultarMateriaPorIdAsync(int idMateria)
        {
            if (idMateria <= 0)
            {
                return null;
            }

            return await materiaRepositorio.ObtenerMateriaPorIdAsync(idMateria);
        }

        // Consulta una materia por su código.
        public async Task<Materia?> ConsultarMateriaPorCodigoAsync(string codigoMateria)
        {
            if (string.IsNullOrWhiteSpace(codigoMateria))
            {
                return null;
            }

            return await materiaRepositorio.ObtenerMateriaPorCodigoAsync(codigoMateria);
        }

        // Lista todas las materias registradas.
        public async Task<List<Materia>> ListarMateriasAsync()
        {
            return await materiaRepositorio.ListarMateriasAsync();
        }

        // Lista únicamente las materias activas.
        public async Task<List<Materia>> ListarMateriasActivasAsync()
        {
            return await materiaRepositorio.ListarMateriasActivasAsync();
        }

        // Desactiva una materia existente.
        public async Task<List<string>> DesactivarMateriaAsync(int idMateria)
        {
            List<string> errores = await ValidarDesactivacionMateriaAsync(idMateria);

            if (errores.Count > 0)
            {
                return errores;
            }

            await materiaRepositorio.DesactivarMateriaAsync(idMateria);

            return errores;
        }

        // Valida las reglas necesarias para crear una materia.
        private async Task<List<string>> ValidarCreacionMateriaAsync(Materia materiaNueva)
        {
            List<string> errores = validadorMateria.Validar(materiaNueva);

            if (materiaNueva == null)
            {
                return errores;
            }

            bool existeCodigo = await materiaRepositorio.ExisteCodigoMateriaAsync(
                materiaNueva.Codigo,
                0
            );

            AgregarErrorSi(
                existeCodigo,
                errores,
                "Ya existe una materia registrada con el mismo código."
            );

            return errores;
        }

        // Valida las reglas necesarias para modificar una materia.
        private async Task<List<string>> ValidarModificacionMateriaAsync(
            int idMateria,
            Materia materiaModificada
        )
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

            bool existeMateria = await materiaRepositorio.ExisteMateriaPorIdAsync(idMateria);

            AgregarErrorSi(
                !existeMateria,
                errores,
                "La materia que desea modificar no existe."
            );

            bool existeCodigo = await materiaRepositorio.ExisteCodigoMateriaAsync(
                materiaModificada.Codigo,
                idMateria
            );

            AgregarErrorSi(
                existeCodigo,
                errores,
                "Ya existe otra materia registrada con el mismo código."
            );

            return errores;
        }

        // Valida que la materia exista antes de desactivarla.
        private async Task<List<string>> ValidarDesactivacionMateriaAsync(int idMateria)
        {
            List<string> errores = new List<string>();

            AgregarErrorSi(
                idMateria <= 0,
                errores,
                "El identificador de la materia no es válido."
            );

            if (errores.Count > 0)
            {
                return errores;
            }

            bool existeMateria = await materiaRepositorio.ExisteMateriaPorIdAsync(idMateria);

            AgregarErrorSi(
                !existeMateria,
                errores,
                "La materia que desea desactivar no existe."
            );

            return errores;
        }

        // Normaliza los datos iniciales de una materia nueva.
        private void PrepararMateriaNueva(Materia materiaNueva)
        {
            materiaNueva.Codigo = materiaNueva.Codigo.Trim();
            materiaNueva.Nombre = materiaNueva.Nombre.Trim();
            materiaNueva.Activa = true;
        }

        // Prepara los datos que serán actualizados en una materia existente.
        private void PrepararMateriaModificada(int idMateria, Materia materiaModificada)
        {
            materiaModificada.IdMateria = idMateria;
            materiaModificada.Codigo = materiaModificada.Codigo.Trim();
            materiaModificada.Nombre = materiaModificada.Nombre.Trim();
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