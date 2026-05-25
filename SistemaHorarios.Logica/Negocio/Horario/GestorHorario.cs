using SistemaHorarios.Datos.Repositorios;
using SistemaHorarios.Modelos.Entidades;
using HorarioEntidad = SistemaHorarios.Modelos.Entidades.Horario;
using DocenteEntidad = SistemaHorarios.Modelos.Entidades.Docente;

namespace SistemaHorarios.Logica.Negocio.Horarios;

// Gestiona las reglas de negocio relacionadas con horarios académicos.
public class GestorHorario
{
    private const string EstadoPendiente = "Pendiente";
    private const string EstadoAprobado = "Aprobado";
    private const string EstadoRechazado = "Rechazado";

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
            await ValidarModificacionAsignaturaAsync(idHorario, horarioModificado);

        if (errores.Count > 0)
        {
            return errores;
        }

        HorarioEntidad? horarioActual =
            await horarioRepository.ObtenerHorarioPorIdAsync(idHorario);

        if (horarioActual == null)
        {
            errores.Add("El horario que desea modificar no existe.");
            return errores;
        }

        horarioModificado.IdGrupo = horarioActual.IdGrupo;
        horarioModificado.Activo = true;
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

    // Desactiva todos los bloques activos de un grupo.
    public async Task<List<string>> DesactivarHorariosPorGrupoAsync(int idGrupo)
    {
        List<string> errores = new List<string>();

        AgregarErrorSi(idGrupo <= 0, errores, "El identificador del grupo no es válido.");

        if (errores.Count > 0)
        {
            return errores;
        }

        bool existeGrupo = await horarioRepository.ExisteGrupoAsync(idGrupo);
        AgregarErrorSi(!existeGrupo, errores, "El grupo no existe o está inactivo.");

        if (errores.Count > 0)
        {
            return errores;
        }

        await horarioRepository.DesactivarHorariosPorGrupoAsync(idGrupo);
        return errores;
    }

    // Aprueba todos los bloques activos del grupo al que pertenece el horario seleccionado.
    public async Task<List<string>> AprobarHorarioAsync(int idHorario)
    {
        List<string> errores = new List<string>();

        HorarioEntidad? horario = await ConsultarHorarioPorIdAsync(idHorario);
        if (horario == null || !horario.Activo)
        {
            errores.Add("El horario no existe o está inactivo.");
            return errores;
        }

        await horarioRepository.ActualizarEstadoHorariosGrupoAsync(
            horario.IdGrupo,
            EstadoAprobado,
            string.Empty);

        return errores;
    }

    // Rechaza todos los bloques activos del grupo al que pertenece el horario seleccionado.
    public async Task<List<string>> RechazarHorarioAsync(int idHorario, string motivoRechazo)
    {
        List<string> errores = new List<string>();

        HorarioEntidad? horario = await ConsultarHorarioPorIdAsync(idHorario);
        if (horario == null || !horario.Activo)
        {
            errores.Add("El horario no existe o está inactivo.");
            return errores;
        }

        string motivo = motivoRechazo.Trim();
        if (string.IsNullOrWhiteSpace(motivo))
        {
            errores.Add("Debe indicar el motivo de rechazo del horario.");
            return errores;
        }

        await horarioRepository.ActualizarEstadoHorariosGrupoAsync(
            horario.IdGrupo,
            EstadoRechazado,
            motivo);

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
        return await horarioRepository.ListarHorariosActivosAsync();
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

    // Genera automáticamente bloques de horario para un grupo basándose en su plan académico.
    public async Task<(int Generados, List<string> Errores)> GenerarHorariosAsync(
        int idGrupo,
        bool reemplazar = true)
    {
        List<string> errores = new List<string>();
        List<HorarioEntidad> horariosGenerados = new List<HorarioEntidad>();

        Grupo? grupo = await horarioRepository.ObtenerGrupoPorIdAsync(idGrupo);
        if (grupo == null)
        {
            errores.Add("El grupo no existe o está inactivo.");
            return (0, errores);
        }

        List<Materia> materias = await horarioRepository.ObtenerMateriasDelGrupoAsync(idGrupo);
        if (materias.Count == 0)
        {
            errores.Add("No hay materias definidas para este grupo en el plan académico.");
            return (0, errores);
        }

        List<FranjaHoraria> franjas = await ObtenerFranjasDisponiblesParaGrupoAsync(grupo);
        if (franjas.Count == 0)
        {
            errores.Add("No hay franjas horarias activas compatibles con la jornada del grupo.");
            return (0, errores);
        }

        foreach (Materia materia in materias)
        {
            int bloquesNecesarios = ObtenerCantidadBloques(materia.IntensidadHorariaSemanal);
            int bloquesMateria = 0;
            List<string> diasMateria = new List<string>();

            List<DocenteEntidad> docentes =
                await horarioRepository.ObtenerDocentesPorMateriaAsync(materia.IdMateria);

            if (docentes.Count == 0)
            {
                errores.Add($"La materia {materia.Nombre} no tiene docentes activos asignados.");
                continue;
            }

            foreach (FranjaHoraria franja in franjas)
            {
                if (bloquesMateria >= bloquesNecesarios)
                {
                    break;
                }

                if (!PuedeUsarDiaParaMateria(diasMateria, franja.DiaSemana))
                {
                    continue;
                }

                if (ExisteCruceEnMemoriaGrupo(horariosGenerados, idGrupo, franja))
                {
                    continue;
                }

                if (!reemplazar && await horarioRepository.ExisteCruceGrupoAsync(idGrupo, franja, 0))
                {
                    continue;
                }

                DocenteEntidad? docenteElegido = await SeleccionarDocenteDisponibleAsync(
                    docentes,
                    franja,
                    horariosGenerados,
                    idGrupo);

                if (docenteElegido == null)
                {
                    continue;
                }

                HorarioEntidad horario = new HorarioEntidad
                {
                    IdGrupo = idGrupo,
                    IdMateria = materia.IdMateria,
                    IdDocente = docenteElegido.IdDocente,
                    IdFranjaHoraria = franja.IdFranjaHoraria,
                    FranjaHoraria = franja,
                    Observacion = "Generado automáticamente",
                    Activo = true,
                    Estado = EstadoPendiente,
                    MotivoRechazo = string.Empty
                };

                horariosGenerados.Add(horario);
                diasMateria.Add(NormalizarDia(franja.DiaSemana));
                bloquesMateria++;
            }

            if (bloquesMateria < bloquesNecesarios)
            {
                errores.Add(
                    $"No fue posible asignar todos los bloques de {materia.Nombre}. " +
                    $"Requeridos: {bloquesNecesarios}, asignados: {bloquesMateria}.");
            }
        }

        if (horariosGenerados.Count == 0)
        {
            errores.Add("No fue posible generar ningún bloque. Revise docentes, materias, disponibilidad y franjas.");
            return (0, errores);
        }

        if (errores.Count > 0)
        {
            return (0, errores);
        }

        if (reemplazar)
        {
            await horarioRepository.DesactivarHorariosPorGrupoAsync(idGrupo);
        }

        await horarioRepository.CrearHorariosAsync(horariosGenerados);

        return (horariosGenerados.Count, errores);
    }

    // Valida las reglas necesarias para crear un horario.
    private async Task<List<string>> ValidarCreacionHorarioAsync(HorarioEntidad horarioNuevo)
    {
        List<string> errores = validadorHorario.Validar(horarioNuevo);

        if (errores.Count > 0)
        {
            return errores;
        }

        await ValidarReglasGeneralesAsync(horarioNuevo, errores, 0);

        return errores;
    }

    // Valida las reglas para modificar solo la asignatura, docente y franja.
    private async Task<List<string>> ValidarModificacionAsignaturaAsync(
        int idHorario,
        HorarioEntidad horarioModificado)
    {
        List<string> errores = validadorHorario.Validar(horarioModificado);

        AgregarErrorSi(idHorario <= 0, errores, "El identificador del horario no es válido.");

        if (errores.Count > 0)
        {
            return errores;
        }

        HorarioEntidad? horarioActual = await horarioRepository.ObtenerHorarioPorIdAsync(idHorario);
        if (horarioActual == null)
        {
            errores.Add("El horario que desea modificar no existe.");
            return errores;
        }

        horarioModificado.IdGrupo = horarioActual.IdGrupo;

        await ValidarReglasGeneralesAsync(horarioModificado, errores, idHorario);

        return errores;
    }

    // Valida las reglas necesarias para modificar un horario.
    private async Task<List<string>> ValidarModificacionHorarioAsync(
        int idHorario,
        HorarioEntidad horarioModificado)
    {
        List<string> errores = validadorHorario.Validar(horarioModificado);

        AgregarErrorSi(idHorario <= 0, errores, "El identificador del horario no es válido.");

        if (errores.Count > 0)
        {
            return errores;
        }

        bool existeHorario = await horarioRepository.ExisteHorarioPorIdAsync(idHorario);
        AgregarErrorSi(!existeHorario, errores, "El horario que desea modificar no existe.");

        if (errores.Count > 0)
        {
            return errores;
        }

        await ValidarReglasGeneralesAsync(horarioModificado, errores, idHorario);

        return errores;
    }

    // Valida que el horario exista antes de desactivarlo.
    private async Task<List<string>> ValidarDesactivacionHorarioAsync(int idHorario)
    {
        List<string> errores = new List<string>();

        AgregarErrorSi(idHorario <= 0, errores, "El identificador del horario no es válido.");

        if (errores.Count > 0)
        {
            return errores;
        }

        bool existeHorario = await horarioRepository.ExisteHorarioPorIdAsync(idHorario);
        AgregarErrorSi(!existeHorario, errores, "El horario que desea desactivar no existe.");

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
            await horarioRepository.ObtenerFranjaHorariaPorIdAsync(horario.IdFranjaHoraria);

        if (franjaHoraria == null)
        {
            errores.Add("La franja horaria no existe.");
            return;
        }

        Grupo? grupo = await horarioRepository.ObtenerGrupoPorIdAsync(horario.IdGrupo);
        if (grupo == null)
        {
            errores.Add("El grupo asociado no existe o está inactivo.");
            return;
        }

        ValidarFranjaInstitucional(grupo, franjaHoraria, errores);

        await ValidarDocenteMateriaAsync(horario, errores);
        await ValidarDisponibilidadDocenteAsync(horario, franjaHoraria, errores);
        await ValidarCruceDocenteAsync(horario, franjaHoraria, errores, idHorarioExcluir);
        await ValidarCruceGrupoAsync(horario, franjaHoraria, errores, idHorarioExcluir);
        await ValidarDistribucionMateriaAsync(horario, franjaHoraria, errores, idHorarioExcluir);
    }

    // Valida que grupo, materia, docente y franja existan y estén activos.
    private async Task ValidarExistenciasAsync(HorarioEntidad horario, List<string> errores)
    {
        bool existeGrupo = await horarioRepository.ExisteGrupoAsync(horario.IdGrupo);
        AgregarErrorSi(!existeGrupo, errores, "El grupo asociado no existe o está inactivo.");

        bool existeMateria = await horarioRepository.ExisteMateriaAsync(horario.IdMateria);
        AgregarErrorSi(!existeMateria, errores, "La materia asociada no existe o está inactiva.");

        bool existeDocente = await horarioRepository.ExisteDocenteAsync(horario.IdDocente);
        AgregarErrorSi(!existeDocente, errores, "El docente asociado no existe o está inactivo.");

        bool existeFranja = await horarioRepository.ExisteFranjaHorariaAsync(horario.IdFranjaHoraria);
        AgregarErrorSi(!existeFranja, errores, "La franja horaria asociada no existe o está inactiva.");
    }

    // Valida que el docente pueda dictar la materia seleccionada.
    private async Task ValidarDocenteMateriaAsync(HorarioEntidad horario, List<string> errores)
    {
        bool puedeDictar = await horarioRepository.ExisteDocenteMateriaAsync(
            horario.IdDocente,
            horario.IdMateria);

        AgregarErrorSi(!puedeDictar, errores, "El docente no está asociado a la materia seleccionada.");
    }

    // Valida que el docente tenga disponibilidad para la franja horaria.
    private async Task ValidarDisponibilidadDocenteAsync(
        HorarioEntidad horario,
        FranjaHoraria franjaHoraria,
        List<string> errores)
    {
        bool tieneDisponibilidad = await horarioRepository.ExisteDisponibilidadDocenteAsync(
            horario.IdDocente,
            franjaHoraria);

        AgregarErrorSi(!tieneDisponibilidad, errores, "El docente no tiene disponibilidad en la franja horaria seleccionada.");
    }

    // Valida que el docente no tenga otro horario cruzado.
    private async Task ValidarCruceDocenteAsync(
        HorarioEntidad horario,
        FranjaHoraria franjaHoraria,
        List<string> errores,
        int idHorarioExcluir)
    {
        bool existeCruce = await horarioRepository.ExisteCruceDocenteAsync(
            horario.IdDocente,
            franjaHoraria,
            idHorarioExcluir);

        AgregarErrorSi(existeCruce, errores, "El docente ya tiene una clase cruzada en esa franja horaria.");
    }

    // Valida que el grupo no tenga otro horario cruzado.
    private async Task ValidarCruceGrupoAsync(
        HorarioEntidad horario,
        FranjaHoraria franjaHoraria,
        List<string> errores,
        int idHorarioExcluir)
    {
        bool existeCruce = await horarioRepository.ExisteCruceGrupoAsync(
            horario.IdGrupo,
            franjaHoraria,
            idHorarioExcluir);

        AgregarErrorSi(existeCruce, errores, "El grupo ya tiene una clase cruzada en esa franja horaria.");
    }

    // Valida que una misma materia no quede el mismo día ni en días seguidos.
    private async Task ValidarDistribucionMateriaAsync(
        HorarioEntidad horario,
        FranjaHoraria franjaHoraria,
        List<string> errores,
        int idHorarioExcluir)
    {
        List<HorarioEntidad> horariosMateria =
            await horarioRepository.ListarHorariosPorGrupoMateriaAsync(
                horario.IdGrupo,
                horario.IdMateria,
                idHorarioExcluir);

        foreach (HorarioEntidad existente in horariosMateria)
        {
            if (existente.FranjaHoraria == null)
            {
                continue;
            }

            if (MismoDia(existente.FranjaHoraria.DiaSemana, franjaHoraria.DiaSemana))
            {
                errores.Add("La misma materia no puede asignarse dos veces el mismo día para el grupo.");
                return;
            }

            if (DiasConsecutivos(existente.FranjaHoraria.DiaSemana, franjaHoraria.DiaSemana))
            {
                errores.Add("La misma materia no puede asignarse en días consecutivos para el grupo.");
                return;
            }
        }
    }

    private async Task<List<FranjaHoraria>> ObtenerFranjasDisponiblesParaGrupoAsync(Grupo grupo)
    {
        List<FranjaHoraria> franjas = await horarioRepository.ObtenerFranjasActivasAsync();

        return franjas
            .Where(f => EsFranjaInstitucionalValida(f))
            .Where(f => EsFranjaCompatibleConJornada(grupo.Jornada, f))
            .ToList();
    }

    private async Task<DocenteEntidad?> SeleccionarDocenteDisponibleAsync(
        List<DocenteEntidad> docentes,
        FranjaHoraria franja,
        List<HorarioEntidad> horariosGenerados,
        int idGrupoIgnorar)
    {
        foreach (DocenteEntidad docente in docentes)
        {
            bool disponible = await horarioRepository.ExisteDisponibilidadDocenteAsync(
                docente.IdDocente,
                franja);

            if (!disponible)
            {
                continue;
            }

            if (ExisteCruceEnMemoriaDocente(horariosGenerados, docente.IdDocente, franja))
            {
                continue;
            }

            bool cruceDocente = await horarioRepository.ExisteCruceDocenteAsync(
                docente.IdDocente,
                franja,
                0,
                idGrupoIgnorar);

            if (cruceDocente)
            {
                continue;
            }

            return docente;
        }

        return null;
    }

    private void ValidarFranjaInstitucional(Grupo grupo, FranjaHoraria franja, List<string> errores)
    {
        if (!EsFranjaInstitucionalValida(franja))
        {
            errores.Add("La franja no es válida. Debe durar 2 horas y no cruzar 12:00-14:00 ni 18:00-18:30.");
            return;
        }

        if (!EsFranjaCompatibleConJornada(grupo.Jornada, franja))
        {
            errores.Add("La franja no corresponde a la jornada del grupo.");
        }
    }

    private static int ObtenerCantidadBloques(int intensidadHorariaSemanal)
    {
        if (intensidadHorariaSemanal <= 0)
        {
            return 1;
        }

        return (int)Math.Ceiling(intensidadHorariaSemanal / 2.0);
    }

    private static bool EsFranjaInstitucionalValida(FranjaHoraria franja)
    {
        bool duraDosHoras = franja.HoraFin - franja.HoraInicio == TimeSpan.FromHours(2);

        return duraDosHoras &&
               !CruzaRango(franja, new TimeSpan(12, 0, 0), new TimeSpan(14, 0, 0)) &&
               !CruzaRango(franja, new TimeSpan(18, 0, 0), new TimeSpan(18, 30, 0));
    }

    private static bool EsFranjaCompatibleConJornada(string jornada, FranjaHoraria franja)
    {
        string jornadaNormalizada = jornada.Trim().ToLower();

        bool esMananaOTarde =
            (franja.HoraInicio >= new TimeSpan(7, 0, 0) && franja.HoraFin <= new TimeSpan(12, 0, 0)) ||
            (franja.HoraInicio >= new TimeSpan(14, 0, 0) && franja.HoraFin <= new TimeSpan(18, 0, 0));

        bool esNoche =
            franja.HoraInicio >= new TimeSpan(18, 30, 0) &&
            franja.HoraFin <= new TimeSpan(22, 30, 0);

        if (jornadaNormalizada.Contains("noct"))
        {
            return esNoche;
        }

        if (jornadaNormalizada.Contains("diurn"))
        {
            return esMananaOTarde;
        }

        return esMananaOTarde || esNoche;
    }

    private static bool CruzaRango(FranjaHoraria franja, TimeSpan inicio, TimeSpan fin)
    {
        return franja.HoraInicio < fin && inicio < franja.HoraFin;
    }

    private static bool ExisteCruceEnMemoriaGrupo(
        List<HorarioEntidad> horarios,
        int idGrupo,
        FranjaHoraria franja)
    {
        return horarios.Any(h =>
            h.IdGrupo == idGrupo &&
            h.FranjaHoraria != null &&
            MismoDia(h.FranjaHoraria.DiaSemana, franja.DiaSemana) &&
            h.FranjaHoraria.HoraInicio < franja.HoraFin &&
            franja.HoraInicio < h.FranjaHoraria.HoraFin);
    }

    private static bool ExisteCruceEnMemoriaDocente(
        List<HorarioEntidad> horarios,
        int idDocente,
        FranjaHoraria franja)
    {
        return horarios.Any(h =>
            h.IdDocente == idDocente &&
            h.FranjaHoraria != null &&
            MismoDia(h.FranjaHoraria.DiaSemana, franja.DiaSemana) &&
            h.FranjaHoraria.HoraInicio < franja.HoraFin &&
            franja.HoraInicio < h.FranjaHoraria.HoraFin);
    }

    private static bool PuedeUsarDiaParaMateria(List<string> diasUsados, string diaCandidato)
    {
        foreach (string diaUsado in diasUsados)
        {
            if (MismoDia(diaUsado, diaCandidato))
            {
                return false;
            }

            if (DiasConsecutivos(diaUsado, diaCandidato))
            {
                return false;
            }
        }

        return true;
    }

    private static bool MismoDia(string diaA, string diaB)
    {
        return NormalizarDia(diaA) == NormalizarDia(diaB);
    }

    private static bool DiasConsecutivos(string diaA, string diaB)
    {
        int ordenA = ObtenerOrdenDia(diaA);
        int ordenB = ObtenerOrdenDia(diaB);

        if (ordenA <= 0 || ordenB <= 0)
        {
            return false;
        }

        return Math.Abs(ordenA - ordenB) == 1;
    }

    private static int ObtenerOrdenDia(string dia)
    {
        return NormalizarDia(dia) switch
        {
            "lunes" => 1,
            "martes" => 2,
            "miercoles" => 3,
            "jueves" => 4,
            "viernes" => 5,
            "sabado" => 6,
            _ => 0
        };
    }

    private static string NormalizarDia(string dia)
    {
        return dia.Trim()
            .ToLower()
            .Replace("á", "a")
            .Replace("é", "e")
            .Replace("í", "i")
            .Replace("ó", "o")
            .Replace("ú", "u");
    }

    // Prepara los datos iniciales de un horario nuevo.
    private void PrepararHorarioNuevo(HorarioEntidad horarioNuevo)
    {
        horarioNuevo.Observacion = horarioNuevo.Observacion.Trim();
        horarioNuevo.Activo = true;
        horarioNuevo.Estado = EstadoPendiente;
        horarioNuevo.MotivoRechazo = string.Empty;
    }

    // Prepara los datos actualizados de un horario existente.
    private void PrepararHorarioModificado(int idHorario, HorarioEntidad horarioModificado)
    {
        horarioModificado.IdHorario = idHorario;
        horarioModificado.Observacion = horarioModificado.Observacion.Trim();
        horarioModificado.Estado = EstadoPendiente;
        horarioModificado.MotivoRechazo = string.Empty;
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
