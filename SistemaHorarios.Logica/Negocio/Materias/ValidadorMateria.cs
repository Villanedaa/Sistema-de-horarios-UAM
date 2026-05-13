using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Materias
{
    // Contiene las validaciones básicas relacionadas con las materias.
    public class ValidadorMateria
    {
        // Valida los datos principales de una materia y devuelve los errores encontrados.
        public List<string> Validar(Materia materia)
        {
            List<string> errores = new List<string>();

            if (materia == null)
            {
                errores.Add("La materia no puede estar vacía.");
                return errores;
            }

            ValidarCodigo(materia.Codigo, errores);
            ValidarNombre(materia.Nombre, errores);
            ValidarCreditos(materia.Creditos, errores);
            ValidarIntensidadHoraria(materia.IntensidadHorariaSemanal, errores);
            ValidarSemestre(materia.Semestre, errores);

            return errores;
        }

        // Valida que el código de la materia sea obligatorio.
        private void ValidarCodigo(string codigo, List<string> errores)
        {
            AgregarErrorSi(
                string.IsNullOrWhiteSpace(codigo),
                errores,
                "El código de la materia es obligatorio."
            );
        }

        // Valida que el nombre de la materia sea obligatorio.
        private void ValidarNombre(string nombre, List<string> errores)
        {
            AgregarErrorSi(
                string.IsNullOrWhiteSpace(nombre),
                errores,
                "El nombre de la materia es obligatorio."
            );
        }

        // Valida que los créditos sean mayores a cero.
        private void ValidarCreditos(int creditos, List<string> errores)
        {
            AgregarErrorSi(
                creditos <= 0,
                errores,
                "El número de créditos debe ser mayor a cero."
            );
        }

        // Valida que la intensidad horaria semanal sea mayor a cero.
        private void ValidarIntensidadHoraria(int intensidadHoraria, List<string> errores)
        {
            AgregarErrorSi(
                intensidadHoraria <= 0,
                errores,
                "La intensidad horaria semanal debe ser mayor a cero."
            );
        }

        // Valida que el semestre sea mayor a cero.
        private void ValidarSemestre(int semestre, List<string> errores)
        {
            AgregarErrorSi(
                semestre <= 0,
                errores,
                "El semestre de la materia debe ser mayor a cero."
            );
        }

        // Agrega un error cuando se cumple una condición inválida.
        private void AgregarErrorSi(bool condicion, List<string> errores, string mensaje)
        {
            if (condicion)
            {
                errores.Add(mensaje);
            }
        }
    }
}