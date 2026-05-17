using HorarioEntidad = SistemaHorarios.Modelos.Entidades.Horario;

namespace SistemaHorarios.Logica.Negocio.Horarios;

// Contiene las validaciones básicas relacionadas con los horarios académicos.
public class ValidadorHorario
{
    // Valida los datos principales de un horario y devuelve los errores encontrados.
    public List<string> Validar(HorarioEntidad horario)
    {
        List<string> errores = new List<string>();

        if (horario == null)
        {
            errores.Add("El horario no puede estar vacío.");
            return errores;
        }

        ValidarGrupo(horario.IdGrupo, errores);
        ValidarMateria(horario.IdMateria, errores);
        ValidarDocente(horario.IdDocente, errores);
        ValidarFranjaHoraria(horario.IdFranjaHoraria, errores);

        return errores;
    }

    // Valida que el grupo tenga un identificador válido.
    private void ValidarGrupo(int idGrupo, List<string> errores)
    {
        AgregarErrorSi(
            idGrupo <= 0,
            errores,
            "El grupo no es válido."
        );
    }

    // Valida que la materia tenga un identificador válido.
    private void ValidarMateria(int idMateria, List<string> errores)
    {
        AgregarErrorSi(
            idMateria <= 0,
            errores,
            "La materia no es válida."
        );
    }

    // Valida que el docente tenga un identificador válido.
    private void ValidarDocente(int idDocente, List<string> errores)
    {
        AgregarErrorSi(
            idDocente <= 0,
            errores,
            "El docente no es válido."
        );
    }

    // Valida que la franja horaria tenga un identificador válido.
    private void ValidarFranjaHoraria(int idFranjaHoraria, List<string> errores)
    {
        AgregarErrorSi(
            idFranjaHoraria <= 0,
            errores,
            "La franja horaria no es válida."
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