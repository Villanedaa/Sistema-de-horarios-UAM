using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Modelos.Entidades;

namespace SistemaHorarios.Datos.Repositorios;

// Gestiona las consultas y operaciones de horarios en la base de datos.
public class HorarioRepository
{
    private readonly SistemaHorariosDbContext contexto;

    // Recibe el contexto de base de datos.
    public HorarioRepository(SistemaHorariosDbContext contexto)
    {
        this.contexto = contexto;
    }

    // Lista todos los horarios registrados.
    public async Task<List<Horario>> ListarHorariosAsync()
    {
        return await ConsultaHorariosBase()
            .OrderBy(h => h.Grupo!.Codigo)
            .ThenBy(h => h.FranjaHoraria!.DiaSemana)
            .ThenBy(h => h.FranjaHoraria!.HoraInicio)
            .ToListAsync();
    }

    // Lista únicamente los horarios activos.
    public async Task<List<Horario>> ListarHorariosActivosAsync()
    {
        return await ConsultaHorariosBase()
            .Where(horario => horario.Activo)
            .OrderBy(h => h.Grupo!.Codigo)
            .ThenBy(h => h.FranjaHoraria!.DiaSemana)
            .ThenBy(h => h.FranjaHoraria!.HoraInicio)
            .ToListAsync();
    }

    // Busca un horario por su identificador.
    public async Task<Horario?> ObtenerHorarioPorIdAsync(int idHorario)
    {
        return await ConsultaHorariosBase()
            .FirstOrDefaultAsync(horario => horario.IdHorario == idHorario);
    }

    // Lista los horarios asociados a un grupo.
    public async Task<List<Horario>> ListarHorariosPorGrupoAsync(int idGrupo)
    {
        return await ConsultaHorariosBase()
            .Where(horario => horario.IdGrupo == idGrupo && horario.Activo)
            .OrderBy(h => h.FranjaHoraria!.DiaSemana)
            .ThenBy(h => h.FranjaHoraria!.HoraInicio)
            .ToListAsync();
    }

    // Lista los horarios asociados a un docente.
    public async Task<List<Horario>> ListarHorariosPorDocenteAsync(int idDocente)
    {
        return await ConsultaHorariosBase()
            .Where(horario => horario.IdDocente == idDocente && horario.Activo)
            .OrderBy(h => h.FranjaHoraria!.DiaSemana)
            .ThenBy(h => h.FranjaHoraria!.HoraInicio)
            .ToListAsync();
    }

    // Lista los horarios asociados a una materia.
    public async Task<List<Horario>> ListarHorariosPorMateriaAsync(int idMateria)
    {
        return await ConsultaHorariosBase()
            .Where(horario => horario.IdMateria == idMateria && horario.Activo)
            .OrderBy(h => h.Grupo!.Codigo)
            .ThenBy(h => h.FranjaHoraria!.DiaSemana)
            .ThenBy(h => h.FranjaHoraria!.HoraInicio)
            .ToListAsync();
    }

    // Busca horarios de un docente por nombre, identificación o correo.
    public async Task<List<Horario>> BuscarHorariosPorDocenteAsync(string busqueda)
    {
        string busquedaLimpia = busqueda.Trim().ToLower();

        return await ConsultaHorariosBase()
            .Where(horario =>
                horario.Activo &&
                horario.Docente != null &&
                (
                    horario.Docente.NombreCompleto.ToLower().Contains(busquedaLimpia) ||
                    horario.Docente.Identificacion.ToLower().Contains(busquedaLimpia) ||
                    horario.Docente.CorreoInstitucional.ToLower().Contains(busquedaLimpia)
                )
            )
            .OrderBy(h => h.Docente!.NombreCompleto)
            .ThenBy(h => h.FranjaHoraria!.DiaSemana)
            .ThenBy(h => h.FranjaHoraria!.HoraInicio)
            .ToListAsync();
    }

    // Verifica si existe un horario por su identificador.
    public async Task<bool> ExisteHorarioPorIdAsync(int idHorario)
    {
        return await contexto.Horarios
            .AnyAsync(horario => horario.IdHorario == idHorario);
    }

    // Verifica si existe el grupo.
    public async Task<bool> ExisteGrupoAsync(int idGrupo)
    {
        return await contexto.Grupos
            .AnyAsync(grupo => grupo.IdGrupo == idGrupo && grupo.Activo);
    }

    // Verifica si existe la materia.
    public async Task<bool> ExisteMateriaAsync(int idMateria)
    {
        return await contexto.Materias
            .AnyAsync(materia => materia.IdMateria == idMateria && materia.Activa);
    }

    // Verifica si existe el docente.
    public async Task<bool> ExisteDocenteAsync(int idDocente)
    {
        return await contexto.Docentes
            .AnyAsync(docente => docente.IdDocente == idDocente && docente.Activo);
    }

    // Verifica si existe la franja horaria.
    public async Task<bool> ExisteFranjaHorariaAsync(int idFranjaHoraria)
    {
        return await contexto.FranjasHorarias
            .AnyAsync(franja => franja.IdFranjaHoraria == idFranjaHoraria && franja.Activa);
    }

    // Obtiene una franja horaria por su identificador.
    public async Task<FranjaHoraria?> ObtenerFranjaHorariaPorIdAsync(int idFranjaHoraria)
    {
        return await contexto.FranjasHorarias
            .FirstOrDefaultAsync(franja => franja.IdFranjaHoraria == idFranjaHoraria);
    }

    // Verifica si un docente puede dictar una materia.
    public async Task<bool> ExisteDocenteMateriaAsync(int idDocente, int idMateria)
    {
        return await contexto.DocenteMaterias
            .AnyAsync(docenteMateria =>
                docenteMateria.IdDocente == idDocente &&
                docenteMateria.IdMateria == idMateria &&
                docenteMateria.Activo);
    }

    // Verifica si el docente está disponible en la franja indicada.
    public async Task<bool> ExisteDisponibilidadDocenteAsync(
        int idDocente,
        FranjaHoraria franjaHoraria)
    {
        return await contexto.DisponibilidadesDocentes
            .AnyAsync(disponibilidad =>
                disponibilidad.IdDocente == idDocente &&
                disponibilidad.Disponible &&
                disponibilidad.Dia.ToLower() == franjaHoraria.DiaSemana.ToLower() &&
                disponibilidad.HoraInicio <= franjaHoraria.HoraInicio &&
                disponibilidad.HoraFin >= franjaHoraria.HoraFin);
    }

    // Verifica cruce real por rango horario del docente, no solo por ID de franja.
    public async Task<bool> ExisteCruceDocenteAsync(
        int idDocente,
        FranjaHoraria franjaHoraria,
        int idHorarioExcluir,
        int idGrupoIgnorar = 0)
    {
        return await contexto.Horarios
            .Include(h => h.FranjaHoraria)
            .AnyAsync(horario =>
                horario.Activo &&
                horario.IdHorario != idHorarioExcluir &&
                horario.IdDocente == idDocente &&
                (idGrupoIgnorar <= 0 || horario.IdGrupo != idGrupoIgnorar) &&
                horario.FranjaHoraria != null &&
                horario.FranjaHoraria.DiaSemana.ToLower() == franjaHoraria.DiaSemana.ToLower() &&
                horario.FranjaHoraria.HoraInicio < franjaHoraria.HoraFin &&
                franjaHoraria.HoraInicio < horario.FranjaHoraria.HoraFin);
    }

    // Verifica cruce real por rango horario del grupo, no solo por ID de franja.
    public async Task<bool> ExisteCruceGrupoAsync(
        int idGrupo,
        FranjaHoraria franjaHoraria,
        int idHorarioExcluir)
    {
        return await contexto.Horarios
            .Include(h => h.FranjaHoraria)
            .AnyAsync(horario =>
                horario.Activo &&
                horario.IdHorario != idHorarioExcluir &&
                horario.IdGrupo == idGrupo &&
                horario.FranjaHoraria != null &&
                horario.FranjaHoraria.DiaSemana.ToLower() == franjaHoraria.DiaSemana.ToLower() &&
                horario.FranjaHoraria.HoraInicio < franjaHoraria.HoraFin &&
                franjaHoraria.HoraInicio < horario.FranjaHoraria.HoraFin);
    }

    // Obtiene los horarios activos de una materia dentro de un grupo.
    public async Task<List<Horario>> ListarHorariosPorGrupoMateriaAsync(
        int idGrupo,
        int idMateria,
        int idHorarioExcluir)
    {
        return await ConsultaHorariosBase()
            .Where(h =>
                h.Activo &&
                h.IdGrupo == idGrupo &&
                h.IdMateria == idMateria &&
                h.IdHorario != idHorarioExcluir)
            .ToListAsync();
    }

    // Guarda un nuevo horario.
    public async Task CrearHorarioAsync(Horario horario)
    {
        await contexto.Horarios.AddAsync(horario);
        await contexto.SaveChangesAsync();
    }

    // Guarda varios horarios en una sola operación.
    public async Task CrearHorariosAsync(List<Horario> horarios)
    {
        if (horarios.Count == 0)
        {
            return;
        }

        await contexto.Horarios.AddRangeAsync(horarios);
        await contexto.SaveChangesAsync();
    }

    // Actualiza un horario existente.
    public async Task<bool> ActualizarHorarioAsync(Horario horarioModificado)
    {
        Horario? horarioActual =
            await contexto.Horarios
                .FirstOrDefaultAsync(horario => horario.IdHorario == horarioModificado.IdHorario);

        if (horarioActual == null)
        {
            return false;
        }

        horarioActual.IdGrupo = horarioModificado.IdGrupo;
        horarioActual.IdMateria = horarioModificado.IdMateria;
        horarioActual.IdDocente = horarioModificado.IdDocente;
        horarioActual.IdFranjaHoraria = horarioModificado.IdFranjaHoraria;
        horarioActual.Observacion = horarioModificado.Observacion;
        horarioActual.Activo = horarioModificado.Activo;
        horarioActual.Estado = horarioModificado.Estado;
        horarioActual.MotivoRechazo = horarioModificado.MotivoRechazo;

        await contexto.SaveChangesAsync();

        return true;
    }

    // Actualiza solo la asignatura, docente y franja de un horario.
    public async Task<bool> ActualizarAsignaturaHorarioAsync(Horario horarioModificado)
    {
        Horario? horarioActual =
            await contexto.Horarios
                .FirstOrDefaultAsync(horario => horario.IdHorario == horarioModificado.IdHorario);

        if (horarioActual == null)
        {
            return false;
        }

        horarioActual.IdMateria = horarioModificado.IdMateria;
        horarioActual.IdDocente = horarioModificado.IdDocente;
        horarioActual.IdFranjaHoraria = horarioModificado.IdFranjaHoraria;
        horarioActual.Observacion = horarioModificado.Observacion;
        horarioActual.Estado = "Pendiente";
        horarioActual.MotivoRechazo = string.Empty;

        await contexto.SaveChangesAsync();

        return true;
    }

    // Desactiva un horario sin eliminarlo físicamente.
    public async Task<bool> DesactivarHorarioAsync(int idHorario)
    {
        Horario? horario =
            await contexto.Horarios
                .FirstOrDefaultAsync(horario => horario.IdHorario == idHorario);

        if (horario == null)
        {
            return false;
        }

        horario.Activo = false;

        await contexto.SaveChangesAsync();

        return true;
    }

    // Desactiva todos los horarios activos de un grupo.
    public async Task<int> DesactivarHorariosPorGrupoAsync(int idGrupo)
    {
        List<Horario> horarios = await contexto.Horarios
            .Where(h => h.IdGrupo == idGrupo && h.Activo)
            .ToListAsync();

        foreach (Horario horario in horarios)
        {
            horario.Activo = false;
        }

        await contexto.SaveChangesAsync();

        return horarios.Count;
    }

    // Actualiza el estado de todos los horarios activos de un grupo.
    public async Task<int> ActualizarEstadoHorariosGrupoAsync(
        int idGrupo,
        string estado,
        string motivoRechazo)
    {
        List<Horario> horarios = await contexto.Horarios
            .Where(h => h.IdGrupo == idGrupo && h.Activo)
            .ToListAsync();

        foreach (Horario horario in horarios)
        {
            horario.Estado = estado;
            horario.MotivoRechazo = motivoRechazo;
        }

        await contexto.SaveChangesAsync();

        return horarios.Count;
    }

    // Obtiene un grupo por su identificador.
    public async Task<Grupo?> ObtenerGrupoPorIdAsync(int idGrupo)
    {
        return await contexto.Grupos
            .FirstOrDefaultAsync(g => g.IdGrupo == idGrupo && g.Activo);
    }

    // Obtiene las materias activas del semestre al que pertenece el grupo.
    public async Task<List<Materia>> ObtenerMateriasDelGrupoAsync(int idGrupo)
    {
        Grupo? grupo = await contexto.Grupos
            .FirstOrDefaultAsync(g => g.IdGrupo == idGrupo && g.Activo);

        if (grupo == null)
        {
            return new List<Materia>();
        }

        SemestrePlan? semestrePlan = await contexto.SemestresPlan
            .FirstOrDefaultAsync(s =>
                s.IdPlanAcademico == grupo.IdPlanAcademico &&
                s.NumeroSemestre == grupo.NumeroSemestre);

        if (semestrePlan == null)
        {
            return new List<Materia>();
        }

        return await contexto.MateriasPlan
            .Include(mp => mp.Materia)
            .Where(mp =>
                mp.IdSemestrePlan == semestrePlan.IdSemestrePlan &&
                mp.Materia != null &&
                mp.Materia.Activa)
            .Select(mp => mp.Materia!)
            .OrderByDescending(m => m.IntensidadHorariaSemanal)
            .ThenBy(m => m.Nombre)
            .ToListAsync();
    }

    // Obtiene los docentes activos asignados a una materia.
    public async Task<List<Docente>> ObtenerDocentesPorMateriaAsync(int idMateria)
    {
        return await contexto.DocenteMaterias
            .Include(dm => dm.Docente)
            .Where(dm =>
                dm.IdMateria == idMateria &&
                dm.Activo &&
                dm.Docente != null &&
                dm.Docente.Activo)
            .Select(dm => dm.Docente!)
            .OrderBy(d => d.NombreCompleto)
            .ToListAsync();
    }

    // Obtiene todas las franjas horarias activas.
    public async Task<List<FranjaHoraria>> ObtenerFranjasActivasAsync()
    {
        List<FranjaHoraria> franjas = await contexto.FranjasHorarias
            .Where(f => f.Activa)
            .ToListAsync();

        return franjas
            .OrderBy(f => ObtenerOrdenDia(f.DiaSemana))
            .ThenBy(f => f.HoraInicio)
            .ToList();
    }

    // Retorna los IDs de franjas ya usadas por un grupo en horarios activos.
    public async Task<HashSet<int>> ObtenerFranjasUsadasPorGrupoAsync(int idGrupo)
    {
        List<int> ids = await contexto.Horarios
            .Where(h => h.IdGrupo == idGrupo && h.Activo)
            .Select(h => h.IdFranjaHoraria)
            .ToListAsync();

        return ids.ToHashSet();
    }

    // Retorna los IDs de franjas ya usadas por un docente en horarios activos.
    public async Task<HashSet<int>> ObtenerFranjasUsadasPorDocenteAsync(int idDocente)
    {
        List<int> ids = await contexto.Horarios
            .Where(h => h.IdDocente == idDocente && h.Activo)
            .Select(h => h.IdFranjaHoraria)
            .ToListAsync();

        return ids.ToHashSet();
    }

    private IQueryable<Horario> ConsultaHorariosBase()
    {
        return contexto.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria);
    }

    private static int ObtenerOrdenDia(string dia)
    {
        return dia.Trim().ToLower() switch
        {
            "lunes" => 1,
            "martes" => 2,
            "miércoles" => 3,
            "miercoles" => 3,
            "jueves" => 4,
            "viernes" => 5,
            "sábado" => 6,
            "sabado" => 6,
            _ => 99
        };
    }
}
