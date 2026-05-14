using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Materias
{
    // Contiene las validaciones básicas relacionadas con los prerrequisitos de una materia.
    public class ValidadorPrerrequisito
    {
        // Valida los datos principales de un prerrequisito y devuelve los errores encontrados.
        public List<string> Validar(Prerrequisito prerrequisito)
        {
            List<string> errores = new List<string>();

            if (prerrequisito == null)
            {
                errores.Add("El prerrequisito no puede estar vacío.");
                return errores;
            }

            ValidarMateriaPrincipal(prerrequisito.IdMateria, errores);
            ValidarMateriaPrerrequisito(prerrequisito.IdMateriaPrerrequisito, errores);
            ValidarMateriasDiferentes(
                prerrequisito.IdMateria,
                prerrequisito.IdMateriaPrerrequisito,
                errores
            );

            return errores;
        }

        // Valida que la materia principal tenga un identificador válido.
        private void ValidarMateriaPrincipal(int idMateria, List<string> errores)
        {
            AgregarErrorSi(
                idMateria <= 0,
                errores,
                "La materia principal no es válida."
            );
        }

        // Valida que la materia prerrequisito tenga un identificador válido.
        private void ValidarMateriaPrerrequisito(int idMateriaPrerrequisito, List<string> errores)
        {
            AgregarErrorSi(
                idMateriaPrerrequisito <= 0,
                errores,
                "La materia prerrequisito no es válida."
            );
        }

        // Valida que una materia no sea prerrequisito de sí misma.
        private void ValidarMateriasDiferentes(
            int idMateria,
            int idMateriaPrerrequisito,
            List<string> errores
        )
        {
            AgregarErrorSi(
                idMateria == idMateriaPrerrequisito,
                errores,
                "Una materia no puede ser prerrequisito de sí misma."
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