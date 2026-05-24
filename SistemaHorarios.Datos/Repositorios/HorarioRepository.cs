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
        return await contexto.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria)
            .ToListAsync();
    }

    // Lista únicamente los horarios activos.
    public async Task<List<Horario>> ListarHorariosActivosAsync()
    {
        return await contexto.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria)
            .Where(horario => horario.Activo)
            .ToListAsync();
    }

    // Busca un horario por su identificador.
    public async Task<Horario?> ObtenerHorarioPorIdAsync(int idHorario)
    {
        return await contexto.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria)
            .FirstOrDefaultAsync(horario => horario.IdHorario == idHorario);
    }

    // Lista los horarios asociados a un grupo.
    public async Task<List<Horario>> ListarHorariosPorGrupoAsync(int idGrupo)
    {
        return await contexto.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria)
            .Where(horario =>
                horario.IdGrupo == idGrupo &&
                horario.Activo
            )
            .ToListAsync();
    }

    // Lista los horarios asociados a un docente.
    public async Task<List<Horario>> ListarHorariosPorDocenteAsync(int idDocente)
    {
        return await contexto.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria)
            .Where(horario =>
                horario.IdDocente == idDocente &&
                horario.Activo
            )
            .ToListAsync();
    }

    // Lista los horarios asociados a una materia.
    public async Task<List<Horario>> ListarHorariosPorMateriaAsync(int idMateria)
    {
        return await contexto.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria)
            .Where(horario =>
                horario.IdMateria == idMateria &&
                horario.Activo
            )
            .ToListAsync();
    }

    // Busca horarios de un docente por nombre, identificación o correo.
    public async Task<List<Horario>> BuscarHorariosPorDocenteAsync(string busqueda)
    {
        string busquedaLimpia = busqueda.Trim().ToLower();

        return await contexto.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria)
            .Where(horario =>
                horario.Activo &&
                horario.Docente != null &&
                (
                    horario.Docente.NombreCompleto.ToLower().Contains(busquedaLimpia) ||
                    horario.Docente.Identificacion.ToLower().Contains(busquedaLimpia) ||
                    horario.Docente.CorreoInstitucional.ToLower().Contains(busquedaLimpia)
                )
            )
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
            .AnyAsync(grupo =>
                grupo.IdGrupo == idGrupo &&
                grupo.Activo
            );
    }

    // Verifica si existe la materia.
    public async Task<bool> ExisteMateriaAsync(int idMateria)
    {
        return await contexto.Materias
            .AnyAsync(materia =>
                materia.IdMateria == idMateria &&
                materia.Activa
            );
    }

    // Verifica si existe el docente.
    public async Task<bool> ExisteDocenteAsync(int idDocente)
    {
        return await contexto.Docentes
            .AnyAsync(docente =>
                docente.IdDocente == idDocente &&
                docente.Activo
            );
    }

    // Verifica si existe la franja horaria.
    public async Task<bool> ExisteFranjaHorariaAsync(int idFranjaHoraria)
    {
        return await contexto.FranjasHorarias
            .AnyAsync(franja =>
                franja.IdFranjaHoraria == idFranjaHoraria &&
                franja.Activa
            );
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
                docenteMateria.Activo
            );
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
                disponibilidad.HoraFin >= franjaHoraria.HoraFin
            );
    }

    // Verifica si el docente ya tiene clase en la misma franja.
    public async Task<bool> ExisteCruceDocenteAsync(
        int idDocente,
        int idFranjaHoraria,
        int idHorarioExcluir)
    {
        return await contexto.Horarios
            .AnyAsync(horario =>
                horario.IdHorario != idHorarioExcluir &&
                horario.IdDocente == idDocente &&
                horario.IdFranjaHoraria == idFranjaHoraria &&
                horario.Activo
            );
    }

    // Verifica si el grupo ya tiene clase en la misma franja.
    public async Task<bool> ExisteCruceGrupoAsync(
        int idGrupo,
        int idFranjaHoraria,
        int idHorarioExcluir)
    {
        return await contexto.Horarios
            .AnyAsync(horario =>
                horario.IdHorario != idHorarioExcluir &&
                horario.IdGrupo == idGrupo &&
                horario.IdFranjaHoraria == idFranjaHoraria &&
                horario.Activo
            );
    }

    // Guarda un nuevo horario.
    public async Task CrearHorarioAsync(Horario horario)
    {
        await contexto.Horarios.AddAsync(horario);
        await contexto.SaveChangesAsync();
    }

    // Actualiza un horario existente.
    public async Task<bool> ActualizarHorarioAsync(Horario horarioModificado)
    {
        Horario? horarioActual =
            await contexto.Horarios
                .FirstOrDefaultAsync(horario =>
                    horario.IdHorario == horarioModificado.IdHorario
                );

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

        await contexto.SaveChangesAsync();

        return true;
    }

    // Actualiza solo la asignatura, docente y franja de un horario.
    public async Task<bool> ActualizarAsignaturaHorarioAsync(Horario horarioModificado)
    {
        Horario? horarioActual =
            await contexto.Horarios
                .FirstOrDefaultAsync(horario =>
                    horario.IdHorario == horarioModificado.IdHorario
                );

        if (horarioActual == null)
        {
            return false;
        }

        horarioActual.IdMateria = horarioModificado.IdMateria;
        horarioActual.IdDocente = horarioModificado.IdDocente;
        horarioActual.IdFranjaHoraria = horarioModificado.IdFranjaHoraria;
        horarioActual.Observacion = horarioModificado.Observacion;

        await contexto.SaveChangesAsync();

        return true;
    }

    // Desactiva un horario sin eliminarlo físicamente.
    public async Task<bool> DesactivarHorarioAsync(int idHorario)
    {
        Horario? horario =
            await contexto.Horarios
                .FirstOrDefaultAsync(horario =>
                    horario.IdHorario == idHorario
                );

        if (horario == null)
        {
            return false;
        }

        horario.Activo = false;

        await contexto.SaveChangesAsync();

        return true;
    }

    // Obtiene un grupo por su identificador.
    public async Task<Grupo?> ObtenerGrupoPorIdAsync(int idGrupo)
    {
        return await contexto.Grupos
            .FirstOrDefaultAsync(g => g.IdGrupo == idGrupo && g.Activo);
    }

    // Obtiene las materias activas del semestre al que pertenece el grupo.
    // Si no hay SemestrePlan configurado, busca por el nombre de materia del grupo.

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
            .ToListAsync();
    }

    // Obtiene todas las franjas horarias activas ordenadas por día y hora.
    public async Task<List<FranjaHoraria>> ObtenerFranjasActivasAsync()
    {
        return await contexto.FranjasHorarias
            .Where(f => f.Activa)
            .OrderBy(f => f.DiaSemana)
            .ThenBy(f => f.HoraInicio)
            .ToListAsync();
    }

    // Retorna los IDs de franjas ya usadas por un grupo (en horarios activos).
    public async Task<HashSet<int>> ObtenerFranjasUsadasPorGrupoAsync(int idGrupo)
    {
        var ids = await contexto.Horarios
            .Where(h => h.IdGrupo == idGrupo && h.Activo)
            .Select(h => h.IdFranjaHoraria)
            .ToListAsync();
        return ids.ToHashSet();
    }

    // Retorna los IDs de franjas ya usadas por un docente (en horarios activos).
    public async Task<HashSet<int>> ObtenerFranjasUsadasPorDocenteAsync(int idDocente)
    {
        var ids = await contexto.Horarios
            .Where(h => h.IdDocente == idDocente && h.Activo)
            .Select(h => h.IdFranjaHoraria)
            .ToListAsync();
        return ids.ToHashSet();
    }
}