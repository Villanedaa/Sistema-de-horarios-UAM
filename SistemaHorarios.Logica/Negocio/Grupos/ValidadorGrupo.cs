using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Logica.Negocio.Grupos;

// Contiene las validaciones básicas relacionadas con los grupos académicos.
public class ValidadorGrupo
{
    // Valida los datos principales de un grupo y devuelve los errores encontrados.
    public List<string> Validar(Grupo grupo)
    {
        List<string> errores = new List<string>();

        if (grupo == null)
        {
            errores.Add("El grupo no puede estar vacío.");
            return errores;
        }

        ValidarCodigo(grupo.Codigo, errores);
        ValidarNombre(grupo.Nombre, errores);
        ValidarJornada(grupo.Jornada, errores);
        ValidarTipoGrupo(grupo.TipoGrupo, errores);
        ValidarNumeroSemestre(grupo.NumeroSemestre, errores);
        ValidarCantidadEstudiantes(grupo.CantidadEstudiantes, errores);
        ValidarPlanAcademico(grupo.IdPlanAcademico, errores);

        return errores;
    }

    // Valida que el código del grupo sea obligatorio.
    private void ValidarCodigo(string codigo, List<string> errores)
    {
        AgregarErrorSi(
            string.IsNullOrWhiteSpace(codigo),
            errores,
            "El código del grupo es obligatorio."
        );
    }

    // Valida que el nombre del grupo sea obligatorio.
    private void ValidarNombre(string nombre, List<string> errores)
    {
        AgregarErrorSi(
            string.IsNullOrWhiteSpace(nombre),
            errores,
            "El nombre del grupo es obligatorio."
        );
    }

    // Valida que la jornada del grupo sea obligatoria.
    private void ValidarJornada(string jornada, List<string> errores)
    {
        AgregarErrorSi(
            string.IsNullOrWhiteSpace(jornada),
            errores,
            "La jornada del grupo es obligatoria."
        );
    }

    // Valida que el tipo de grupo sea obligatorio.
    private void ValidarTipoGrupo(string tipoGrupo, List<string> errores)
    {
        AgregarErrorSi(
            string.IsNullOrWhiteSpace(tipoGrupo),
            errores,
            "El tipo de grupo es obligatorio."
        );
    }

    // Valida que el número de semestre sea mayor a cero.
    private void ValidarNumeroSemestre(int numeroSemestre, List<string> errores)
    {
        AgregarErrorSi(
            numeroSemestre <= 0,
            errores,
            "El número de semestre debe ser mayor a cero."
        );
    }

    // Valida que la cantidad de estudiantes no sea negativa.
    private void ValidarCantidadEstudiantes(int cantidadEstudiantes, List<string> errores)
    {
        AgregarErrorSi(
            cantidadEstudiantes < 0,
            errores,
            "La cantidad de estudiantes no puede ser negativa."
        );
    }

    // Valida temporalmente que el plan académico tenga un identificador válido.
    private void ValidarPlanAcademico(int idPlanAcademico, List<string> errores)
    {
        AgregarErrorSi(
            idPlanAcademico <= 0,
            errores,
            "El plan académico asociado no es válido."
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