using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Modelos.Entidades;
using HorarioEntidad = SistemaHorarios.Modelos.Entidades.Horario;

namespace SistemaHorarios.Logica.Negocio.Horarios;

// Gestiona las reglas de negocio relacionadas con horarios académicos.
public class GestorHorario
{
    private readonly HorarioRepository horarioRepository;
    private readonly ValidadorHorario validadorHorario;

    // Recibe el repositorio de horarios para trabajar con base de datos.
    public GestorHorario(HorarioRepository horarioRepository)
    {
        this.horarioRepository = horarioRepository;
        validadorHorario = new ValidadorHorario();
    }

    // Crea un nuevo horario después de validar reglas y conflictos.
    public async Task<List<string>> CrearHorarioAsync(HorarioEntidad horarioNuevo)
    {
        List<string> errores = await ValidarCreacionHorarioAsync(horarioNuevo);

        if (errores.Count > 0)
        {
            return errores;
        }

        PrepararHorarioNuevo(horarioNuevo);

        await horarioRepository.CrearHorarioAsync(horarioNuevo);

        return errores;
    }

    // Modifica un horario existente después de validar reglas y conflictos.
    public async Task<List<string>> ModificarHorarioAsync(
        int idHorario,
        HorarioEntidad horarioModificado)
    {
        List<string> errores =
            await ValidarModificacionHorarioAsync(idHorario, horarioModificado);

        if (errores.Count > 0)
        {
            return errores;
        }

        PrepararHorarioModificado(idHorario, horarioModificado);

        await horarioRepository.ActualizarHorarioAsync(horarioModificado);

        return errores;
    }

    // Modifica solamente la materia, docente y franja de un horario.
    public async Task<List<string>> ModificarAsignaturaHorarioAsync(
        int idHorario,
        HorarioEntidad horarioModificado)
    {
        List<string> errores =
            await ValidarModificacionHorarioAsync(idHorario, horarioModificado);

        if (errores.Count > 0)
        {
            return errores;
        }

        PrepararHorarioModificado(idHorario, horarioModificado);

        await horarioRepository.ActualizarAsignaturaHorarioAsync(horarioModificado);

        return errores;
    }

    // Desactiva un horario sin eliminarlo físicamente.
    public async Task<List<string>> DesactivarHorarioAsync(int idHorario)
    {
        List<string> errores = await ValidarDesactivacionHorarioAsync(idHorario);

        if (errores.Count > 0)
        {
            return errores;
        }

        await horarioRepository.DesactivarHorarioAsync(idHorario);

        return errores;
    }

    // Consulta un horario por su identificador.
    public async Task<HorarioEntidad?> ConsultarHorarioPorIdAsync(int idHorario)
    {
        if (idHorario <= 0)
        {
            return null;
        }

        return await horarioRepository.ObtenerHorarioPorIdAsync(idHorario);
    }

    // Lista todos los horarios registrados.
    public async Task<List<HorarioEntidad>> ListarHorariosAsync()
    {
        return await horarioRepository.ListarHorariosAsync();
    }

    // Lista únicamente los horarios activos.
    public async Task<List<HorarioEntidad>> ListarHorariosActivosAsync()
    {
        return await horarioRepository.ListarHorariosActivosAsync();
    }

    // Lista los horarios asociados a un grupo.
    public async Task<List<HorarioEntidad>> ListarHorariosPorGrupoAsync(int idGrupo)
    {
        if (idGrupo <= 0)
        {
            return new List<HorarioEntidad>();
        }

        return await horarioRepository.ListarHorariosPorGrupoAsync(idGrupo);
    }

    // Lista los horarios asociados a un docente.
    public async Task<List<HorarioEntidad>> ListarHorariosPorDocenteAsync(int idDocente)
    {
        if (idDocente <= 0)
        {
            return new List<HorarioEntidad>();
        }

        return await horarioRepository.ListarHorariosPorDocenteAsync(idDocente);
    }

    // Lista los horarios asociados a una materia.
    public async Task<List<HorarioEntidad>> ListarHorariosPorMateriaAsync(int idMateria)
    {
        if (idMateria <= 0)
        {
            return new List<HorarioEntidad>();
        }

        return await horarioRepository.ListarHorariosPorMateriaAsync(idMateria);
    }

    // Busca horarios por nombre, identificación o correo del docente.
    public async Task<List<HorarioEntidad>> BuscarHorariosPorDocenteAsync(string busqueda)
    {
        if (string.IsNullOrWhiteSpace(busqueda))
        {
            return new List<HorarioEntidad>();
        }

        return await horarioRepository.BuscarHorariosPorDocenteAsync(busqueda);
    }

    // Valida las reglas necesarias para crear un horario.
    private async Task<List<string>> ValidarCreacionHorarioAsync(
        HorarioEntidad horarioNuevo)
    {
        List<string> errores = validadorHorario.Validar(horarioNuevo);

        if (horarioNuevo == null)
        {
            return errores;
        }

        await ValidarReglasGeneralesAsync(horarioNuevo, errores, 0);

        return errores;
    }

    // Valida las reglas necesarias para modificar un horario.
    private async Task<List<string>> ValidarModificacionHorarioAsync(
        int idHorario,
        HorarioEntidad horarioModificado)
    {
        List<string> errores = validadorHorario.Validar(horarioModificado);

        AgregarErrorSi(
            idHorario <= 0,
            errores,
            "El identificador del horario no es válido."
        );

        if (horarioModificado == null)
        {
            return errores;
        }

        bool existeHorario =
            await horarioRepository.ExisteHorarioPorIdAsync(idHorario);

        AgregarErrorSi(
            !existeHorario,
            errores,
            "El horario que desea modificar no existe."
        );

        await ValidarReglasGeneralesAsync(horarioModificado, errores, idHorario);

        return errores;
    }

    // Valida que el horario exista antes de desactivarlo.
    private async Task<List<string>> ValidarDesactivacionHorarioAsync(int idHorario)
    {
        List<string> errores = new List<string>();

        AgregarErrorSi(
            idHorario <= 0,
            errores,
            "El identificador del horario no es válido."
        );

        if (errores.Count > 0)
        {
            return errores;
        }

        bool existeHorario =
            await horarioRepository.ExisteHorarioPorIdAsync(idHorario);

        AgregarErrorSi(
            !existeHorario,
            errores,
            "El horario que desea desactivar no existe."
        );

        return errores;
    }

    // Valida reglas de negocio que requieren consultar base de datos.
    private async Task ValidarReglasGeneralesAsync(
        HorarioEntidad horario,
        List<string> errores,
        int idHorarioExcluir)
    {
        await ValidarExistenciasAsync(horario, errores);

        if (errores.Count > 0)
        {
            return;
        }

        FranjaHoraria? franjaHoraria =
            await horarioRepository.ObtenerFranjaHorariaPorIdAsync(
                horario.IdFranjaHoraria
            );

        if (franjaHoraria == null)
        {
            errores.Add("La franja horaria no existe.");
            return;
        }

        await ValidarDocenteMateriaAsync(horario, errores);
        await ValidarDisponibilidadDocenteAsync(horario, franjaHoraria, errores);
        await ValidarCruceDocenteAsync(horario, errores, idHorarioExcluir);
        await ValidarCruceGrupoAsync(horario, errores, idHorarioExcluir);
    }

    // Valida que grupo, materia, docente y franja existan y estén activos.
    private async Task ValidarExistenciasAsync(
        HorarioEntidad horario,
        List<string> errores)
    {
        bool existeGrupo =
            await horarioRepository.ExisteGrupoAsync(horario.IdGrupo);

        AgregarErrorSi(
            !existeGrupo,
            errores,
            "El grupo asociado no existe o está inactivo."
        );

        bool existeMateria =
            await horarioRepository.ExisteMateriaAsync(horario.IdMateria);

        AgregarErrorSi(
            !existeMateria,
            errores,
            "La materia asociada no existe o está inactiva."
        );

        bool existeDocente =
            await horarioRepository.ExisteDocenteAsync(horario.IdDocente);

        AgregarErrorSi(
            !existeDocente,
            errores,
            "El docente asociado no existe o está inactivo."
        );

        bool existeFranja =
            await horarioRepository.ExisteFranjaHorariaAsync(
                horario.IdFranjaHoraria
            );

        AgregarErrorSi(
            !existeFranja,
            errores,
            "La franja horaria asociada no existe o está inactiva."
        );
    }

    // Valida que el docente pueda dictar la materia seleccionada.
    private async Task ValidarDocenteMateriaAsync(
        HorarioEntidad horario,
        List<string> errores)
    {
        bool puedeDictar =
            await horarioRepository.ExisteDocenteMateriaAsync(
                horario.IdDocente,
                horario.IdMateria
            );

        AgregarErrorSi(
            !puedeDictar,
            errores,
            "El docente no está asociado a la materia seleccionada."
        );
    }

    // Valida que el docente tenga disponibilidad para la franja horaria.
    private async Task ValidarDisponibilidadDocenteAsync(
        HorarioEntidad horario,
        FranjaHoraria franjaHoraria,
        List<string> errores)
    {
        bool tieneDisponibilidad =
            await horarioRepository.ExisteDisponibilidadDocenteAsync(
                horario.IdDocente,
                franjaHoraria
            );

        AgregarErrorSi(
            !tieneDisponibilidad,
            errores,
            "El docente no tiene disponibilidad en la franja horaria seleccionada."
        );
    }

    // Valida que el docente no tenga otro horario en la misma franja.
    private async Task ValidarCruceDocenteAsync(
        HorarioEntidad horario,
        List<string> errores,
        int idHorarioExcluir)
    {
        bool existeCruce =
            await horarioRepository.ExisteCruceDocenteAsync(
                horario.IdDocente,
                horario.IdFranjaHoraria,
                idHorarioExcluir
            );

        AgregarErrorSi(
            existeCruce,
            errores,
            "El docente ya tiene una clase asignada en esa franja horaria."
        );
    }

    // Valida que el grupo no tenga otro horario en la misma franja.
    private async Task ValidarCruceGrupoAsync(
        HorarioEntidad horario,
        List<string> errores,
        int idHorarioExcluir)
    {
        bool existeCruce =
            await horarioRepository.ExisteCruceGrupoAsync(
                horario.IdGrupo,
                horario.IdFranjaHoraria,
                idHorarioExcluir
            );

        AgregarErrorSi(
            existeCruce,
            errores,
            "El grupo ya tiene una clase asignada en esa franja horaria."
        );
    }

    // Prepara los datos iniciales de un horario nuevo.
    private void PrepararHorarioNuevo(HorarioEntidad horarioNuevo)
    {
        horarioNuevo.Observacion = horarioNuevo.Observacion.Trim();
        horarioNuevo.Activo = true;
    }

    // Prepara los datos actualizados de un horario existente.
    private void PrepararHorarioModificado(
        int idHorario,
        HorarioEntidad horarioModificado)
    {
        horarioModificado.IdHorario = idHorario;
        horarioModificado.Observacion = horarioModificado.Observacion.Trim();
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