using Microsoft.EntityFrameworkCore;
using SistemaHorarios.Datos.Contexto;
using SistemaHorarios.Logica.Negocio.Reportes.Interfaces;
using SistemaHorarios.Modelos.DTOs.Reportes;
using SistemaHorarios.Modelos.Entidades;
using System.Globalization;
using System.Text;

namespace SistemaHorarios.Logica.Negocio.Reportes.Servicios;

public class ReporteService : IReporteService
{
    private readonly SistemaHorariosDbContext _context;

    public ReporteService(SistemaHorariosDbContext context)
    {
        _context = context;
    }

    public async Task<ReporteGeneralDto> ObtenerReporteGeneralAsync()
    {
        var reporte = new ReporteGeneralDto
        {
            TotalUsuarios = await _context.Usuarios.CountAsync(),
            TotalRoles = await _context.Roles.CountAsync(),
            TotalMaterias = await _context.Materias.CountAsync(),
            TotalPrerrequisitos = await _context.Prerrequisitos.CountAsync(),
            TotalFranjasHorarias = await _context.FranjasHorarias.CountAsync(),
            TotalPlanesAcademicos = await _context.PlanesAcademicos.CountAsync(),
            TotalSemestresPlan = await _context.SemestresPlan.CountAsync(),
            TotalMateriasPlan = await _context.MateriasPlan.CountAsync()
        };

        return reporte;
    }

    public async Task<List<ReporteUsuariosPorRolDto>> ObtenerUsuariosPorRolAsync()
    {
        var usuarios =
            await _context.Usuarios
                .Include(usuario => usuario.Rol)
                .ToListAsync();

        return usuarios
            .GroupBy(usuario => usuario.Rol != null ? usuario.Rol.Nombre : "Sin rol")
            .Select(grupo => new ReporteUsuariosPorRolDto
            {
                Rol = grupo.Key,
                TotalUsuarios = grupo.Count()
            })
            .OrderBy(item => item.Rol)
            .ToList();
    }

    public async Task<List<ReporteMateriasPorSemestreDto>> ObtenerMateriasPorSemestreAsync()
    {
        var materias = await _context.Materias.ToListAsync();

        return materias
            .GroupBy(materia => materia.Semestre)
            .OrderBy(grupo => grupo.Key)
            .Select(grupo => new ReporteMateriasPorSemestreDto
            {
                Semestre = grupo.Key,
                TotalMaterias = grupo.Count(),
                Materias = grupo
                    .OrderBy(materia => materia.Nombre)
                    .Select(materia => materia.Nombre)
                    .ToList()
            })
            .ToList();
    }

    public async Task<List<ReporteFranjaHorariaDto>> ObtenerFranjasPorDiaAsync()
    {
        var franjas = await _context.FranjasHorarias.ToListAsync();

        return franjas
            .GroupBy(franja => franja.DiaSemana)
            .Select(grupo => new ReporteFranjaHorariaDto
            {
                Dia = grupo.Key,
                TotalFranjas = grupo.Count()
            })
            .OrderBy(item => OrdenDia(item.Dia))
            .ToList();
    }

    public async Task<List<ReportePlanAcademicoDto>> ObtenerPlanesAcademicosAsync()
    {
        var planes =
            await _context.PlanesAcademicos
                .Include(plan => plan.Semestres)
                    .ThenInclude(semestre => semestre.MateriasPlan)
                .ToListAsync();

        return planes
            .Select(plan => new ReportePlanAcademicoDto
            {
                IdPlanAcademico = plan.IdPlanAcademico,
                Nombre = plan.Nombre,
                Programa = plan.Programa,
                Anio = plan.Anio,
                TotalSemestres = plan.Semestres.Count,
                TotalMaterias = plan.Semestres.Sum(semestre => semestre.MateriasPlan.Count)
            })
            .OrderBy(item => item.Programa)
            .ThenBy(item => item.Anio)
            .ToList();
    }

    public async Task<List<ReporteHorarioGrupoDto>> ObtenerHorarioPorGrupoAsync(int idGrupo)
    {
        var horarios = await _context.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria)
            .Where(horario => horario.Activo && horario.IdGrupo == idGrupo)
            .ToListAsync();

        return horarios
            .Select(horario => new ReporteHorarioGrupoDto
            {
                IdHorario = horario.IdHorario,
                IdGrupo = horario.IdGrupo,
                CodigoGrupo = horario.Grupo != null ? horario.Grupo.Codigo : string.Empty,
                NombreGrupo = horario.Grupo != null ? horario.Grupo.Nombre : string.Empty,
                Jornada = horario.Grupo != null ? horario.Grupo.Jornada : string.Empty,
                NumeroSemestre = horario.Grupo != null ? horario.Grupo.NumeroSemestre : 0,
                CodigoMateria = horario.Materia != null ? horario.Materia.Codigo : string.Empty,
                Materia = horario.Materia != null ? horario.Materia.Nombre : string.Empty,
                Docente = horario.Docente != null ? horario.Docente.NombreCompleto : string.Empty,
                DiaSemana = horario.FranjaHoraria != null ? horario.FranjaHoraria.DiaSemana : string.Empty,
                HoraInicio = horario.FranjaHoraria != null ? FormatearHora(horario.FranjaHoraria.HoraInicio) : string.Empty,
                HoraFin = horario.FranjaHoraria != null ? FormatearHora(horario.FranjaHoraria.HoraFin) : string.Empty,
                Estado = horario.Estado,
                Observacion = horario.Observacion
            })
            .OrderBy(item => OrdenDia(item.DiaSemana))
            .ThenBy(item => item.HoraInicio)
            .ThenBy(item => item.Materia)
            .ToList();
    }

    public async Task<List<ReporteHorarioDocenteDto>> ObtenerHorarioPorDocenteAsync(int idDocente)
    {
        var horarios = await _context.Horarios
            .Include(horario => horario.Grupo)
            .Include(horario => horario.Materia)
            .Include(horario => horario.Docente)
            .Include(horario => horario.FranjaHoraria)
            .Where(horario => horario.Activo && horario.IdDocente == idDocente)
            .ToListAsync();

        return horarios
            .Select(horario => new ReporteHorarioDocenteDto
            {
                IdHorario = horario.IdHorario,
                IdDocente = horario.IdDocente,
                Docente = horario.Docente != null ? horario.Docente.NombreCompleto : string.Empty,
                CodigoGrupo = horario.Grupo != null ? horario.Grupo.Codigo : string.Empty,
                NombreGrupo = horario.Grupo != null ? horario.Grupo.Nombre : string.Empty,
                Jornada = horario.Grupo != null ? horario.Grupo.Jornada : string.Empty,
                NumeroSemestre = horario.Grupo != null ? horario.Grupo.NumeroSemestre : 0,
                CodigoMateria = horario.Materia != null ? horario.Materia.Codigo : string.Empty,
                Materia = horario.Materia != null ? horario.Materia.Nombre : string.Empty,
                DiaSemana = horario.FranjaHoraria != null ? horario.FranjaHoraria.DiaSemana : string.Empty,
                HoraInicio = horario.FranjaHoraria != null ? FormatearHora(horario.FranjaHoraria.HoraInicio) : string.Empty,
                HoraFin = horario.FranjaHoraria != null ? FormatearHora(horario.FranjaHoraria.HoraFin) : string.Empty,
                Estado = horario.Estado
            })
            .OrderBy(item => OrdenDia(item.DiaSemana))
            .ThenBy(item => item.HoraInicio)
            .ThenBy(item => item.CodigoGrupo)
            .ToList();
    }

    public async Task<List<ReporteCargaDocenteDto>> ObtenerCargaDocenteAsync()
    {
        var docentes =
            await _context.Docentes
                .OrderBy(docente => docente.NombreCompleto)
                .ToListAsync();

        var horarios =
            await _context.Horarios
                .Where(horario => horario.Activo)
                .ToListAsync();

        return docentes
            .Select(docente =>
            {
                var horariosDocente = horarios
                    .Where(horario => horario.IdDocente == docente.IdDocente)
                    .ToList();

                return new ReporteCargaDocenteDto
                {
                    IdDocente = docente.IdDocente,
                    Docente = docente.NombreCompleto,
                    EstadoDocente = docente.Activo ? "Activo" : "Inactivo",
                    CantidadMaterias = horariosDocente
                        .Select(horario => horario.IdMateria)
                        .Distinct()
                        .Count(),
                    CantidadGrupos = horariosDocente
                        .Select(horario => horario.IdGrupo)
                        .Distinct()
                        .Count(),
                    BloquesSemanales = horariosDocente.Count,
                    HorasSemanales = horariosDocente.Count * 2
                };
            })
            .ToList();
    }

    public async Task<List<ReporteConflictoHorarioDto>> ObtenerConflictosHorarioAsync()
    {
        var conflictos = new List<ReporteConflictoHorarioDto>();

        var horarios =
            await _context.Horarios
                .Include(horario => horario.Grupo)
                .Include(horario => horario.Docente)
                .Include(horario => horario.Materia)
                .Include(horario => horario.FranjaHoraria)
                .Where(horario => horario.Activo)
                .ToListAsync();

        var disponibilidades =
            await _context.DisponibilidadesDocentes
                .Where(disponibilidad => disponibilidad.Disponible)
                .ToListAsync();

        var docenteMaterias =
            await _context.DocenteMaterias
                .Where(docenteMateria => docenteMateria.Activo)
                .ToListAsync();

        var materiasPlan =
            await _context.MateriasPlan
                .Include(materiaPlan => materiaPlan.SemestrePlan)
                .ToListAsync();

        DetectarSolapesGrupo(horarios, conflictos);
        DetectarSolapesDocente(horarios, conflictos);
        DetectarDocenteSinDisponibilidad(horarios, disponibilidades, conflictos);
        DetectarDocenteNoDictaMateria(horarios, docenteMaterias, conflictos);
        DetectarMateriaFueraPlan(horarios, materiasPlan, conflictos);

        return conflictos
            .OrderBy(conflicto => conflicto.TipoConflicto)
            .ThenBy(conflicto => conflicto.Grupo)
            .ThenBy(conflicto => OrdenDia(conflicto.DiaSemana))
            .ThenBy(conflicto => conflicto.HoraInicio)
            .ToList();
    }

    public async Task<List<ReporteHorarioGeneradoDto>> ObtenerHorariosGeneradosAsync()
    {
        var horarios =
            await _context.Horarios
                .Include(horario => horario.Grupo)
                .Where(horario => horario.Activo)
                .ToListAsync();

        return horarios
            .Where(horario => horario.Grupo != null)
            .GroupBy(horario => horario.Grupo!)
            .Select(grupo => new ReporteHorarioGeneradoDto
            {
                IdGrupo = grupo.Key.IdGrupo,
                CodigoGrupo = grupo.Key.Codigo,
                NombreGrupo = grupo.Key.Nombre,
                Jornada = grupo.Key.Jornada,
                NumeroSemestre = grupo.Key.NumeroSemestre,
                TotalMaterias = grupo.Select(horario => horario.IdMateria).Distinct().Count(),
                TotalDocentes = grupo.Select(horario => horario.IdDocente).Distinct().Count(),
                TotalBloques = grupo.Count(),
                TotalHoras = grupo.Count() * 2,
                Estado = ObtenerEstadoGrupo(grupo.Select(horario => horario.Estado).ToList())
            })
            .OrderBy(item => item.Jornada)
            .ThenBy(item => item.NumeroSemestre)
            .ThenBy(item => item.CodigoGrupo)
            .ToList();
    }

    public async Task<(byte[] Archivo, string ContentType, string NombreArchivo)> ExportarReporteAsync(
        string tipo,
        string formato,
        int? idGrupo,
        int? idDocente)
    {
        string tipoNormalizado = Normalizar(tipo);
        string formatoNormalizado = Normalizar(formato);

        var lineas = await ConstruirLineasReporteAsync(tipoNormalizado, idGrupo, idDocente);

        if (lineas.Count == 0)
        {
            lineas.Add("No se encontraron datos para el reporte solicitado.");
        }

        string nombreBase = $"reporte_{tipoNormalizado}_{DateTime.Now:yyyyMMdd_HHmmss}";

        if (formatoNormalizado == "pdf")
        {
            return (
                CrearPdfSimple(lineas),
                "application/pdf",
                $"{nombreBase}.pdf");
        }

        return (
            CrearCsvSimple(lineas),
            "text/csv",
            $"{nombreBase}.csv");
    }

    private async Task<List<string>> ConstruirLineasReporteAsync(
        string tipo,
        int? idGrupo,
        int? idDocente)
    {
        if (tipo is "horariogrupo" or "horario-grupo" or "asignacionporgrupo")
        {
            if (!idGrupo.HasValue)
            {
                throw new InvalidOperationException("Debe seleccionar un grupo para exportar el horario por grupo.");
            }

            var datos = await ObtenerHorarioPorGrupoAsync(idGrupo.Value);
            var lineas = new List<string>
            {
                "Horario por grupo",
                "Grupo;Jornada;Semestre;Dia;Hora inicio;Hora fin;Materia;Docente;Estado"
            };

            lineas.AddRange(datos.Select(item =>
                UnirCsv(
                    item.CodigoGrupo,
                    item.Jornada,
                    item.NumeroSemestre.ToString(),
                    item.DiaSemana,
                    item.HoraInicio,
                    item.HoraFin,
                    item.Materia,
                    item.Docente,
                    item.Estado)));

            return lineas;
        }

        if (tipo is "horariodocente" or "horario-docente")
        {
            if (!idDocente.HasValue)
            {
                throw new InvalidOperationException("Debe seleccionar un docente para exportar el horario por docente.");
            }

            var datos = await ObtenerHorarioPorDocenteAsync(idDocente.Value);
            var lineas = new List<string>
            {
                "Horario por docente",
                "Docente;Grupo;Jornada;Semestre;Dia;Hora inicio;Hora fin;Materia;Estado"
            };

            lineas.AddRange(datos.Select(item =>
                UnirCsv(
                    item.Docente,
                    item.CodigoGrupo,
                    item.Jornada,
                    item.NumeroSemestre.ToString(),
                    item.DiaSemana,
                    item.HoraInicio,
                    item.HoraFin,
                    item.Materia,
                    item.Estado)));

            return lineas;
        }

        if (tipo is "cargadocente" or "carga-docente")
        {
            var datos = await ObtenerCargaDocenteAsync();
            var lineas = new List<string>
            {
                "Carga docente",
                "Docente;Estado;Materias;Grupos;Bloques semanales;Horas semanales"
            };

            lineas.AddRange(datos.Select(item =>
                UnirCsv(
                    item.Docente,
                    item.EstadoDocente,
                    item.CantidadMaterias.ToString(),
                    item.CantidadGrupos.ToString(),
                    item.BloquesSemanales.ToString(),
                    item.HorasSemanales.ToString())));

            return lineas;
        }

        if (tipo is "conflictoshorario" or "conflictos-horario")
        {
            var datos = await ObtenerConflictosHorarioAsync();
            var lineas = new List<string>
            {
                "Conflictos de horario",
                "Tipo;Descripcion;Grupo;Docente;Materia;Dia;Hora inicio;Hora fin"
            };

            if (datos.Count == 0)
            {
                lineas.Add("Sin conflictos;No se encontraron conflictos activos;;;;;;");
                return lineas;
            }

            lineas.AddRange(datos.Select(item =>
                UnirCsv(
                    item.TipoConflicto,
                    item.Descripcion,
                    item.Grupo,
                    item.Docente,
                    item.Materia,
                    item.DiaSemana,
                    item.HoraInicio,
                    item.HoraFin)));

            return lineas;
        }

        var horarios = await ObtenerHorariosGeneradosAsync();
        var lineasGenerales = new List<string>
        {
            "Horarios generados",
            "Grupo;Nombre;Jornada;Semestre;Materias;Docentes;Bloques;Horas;Estado"
        };

        lineasGenerales.AddRange(horarios.Select(item =>
            UnirCsv(
                item.CodigoGrupo,
                item.NombreGrupo,
                item.Jornada,
                item.NumeroSemestre.ToString(),
                item.TotalMaterias.ToString(),
                item.TotalDocentes.ToString(),
                item.TotalBloques.ToString(),
                item.TotalHoras.ToString(),
                item.Estado)));

        return lineasGenerales;
    }

    private static void DetectarSolapesGrupo(
        List<Horario> horarios,
        List<ReporteConflictoHorarioDto> conflictos)
    {
        foreach (var horario in horarios)
        {
            if (horario.FranjaHoraria == null)
            {
                continue;
            }

            bool existeSolape = horarios.Any(otro =>
                otro.IdHorario != horario.IdHorario &&
                otro.IdGrupo == horario.IdGrupo &&
                otro.FranjaHoraria != null &&
                MismoDia(otro.FranjaHoraria.DiaSemana, horario.FranjaHoraria.DiaSemana) &&
                HaySolape(
                    horario.FranjaHoraria.HoraInicio,
                    horario.FranjaHoraria.HoraFin,
                    otro.FranjaHoraria.HoraInicio,
                    otro.FranjaHoraria.HoraFin));

            if (existeSolape)
            {
                AgregarConflicto(
                    conflictos,
                    "Solape por grupo",
                    "El grupo tiene más de una clase en la misma franja.",
                    horario);
            }
        }
    }

    private static void DetectarSolapesDocente(
        List<Horario> horarios,
        List<ReporteConflictoHorarioDto> conflictos)
    {
        foreach (var horario in horarios)
        {
            if (horario.FranjaHoraria == null)
            {
                continue;
            }

            bool existeSolape = horarios.Any(otro =>
                otro.IdHorario != horario.IdHorario &&
                otro.IdDocente == horario.IdDocente &&
                otro.FranjaHoraria != null &&
                MismoDia(otro.FranjaHoraria.DiaSemana, horario.FranjaHoraria.DiaSemana) &&
                HaySolape(
                    horario.FranjaHoraria.HoraInicio,
                    horario.FranjaHoraria.HoraFin,
                    otro.FranjaHoraria.HoraInicio,
                    otro.FranjaHoraria.HoraFin));

            if (existeSolape)
            {
                AgregarConflicto(
                    conflictos,
                    "Solape por docente",
                    "El docente tiene más de una clase en la misma franja.",
                    horario);
            }
        }
    }

    private static void DetectarDocenteSinDisponibilidad(
        List<Horario> horarios,
        List<DisponibilidadDocente> disponibilidades,
        List<ReporteConflictoHorarioDto> conflictos)
    {
        foreach (var horario in horarios)
        {
            if (horario.FranjaHoraria == null)
            {
                continue;
            }

            bool tieneDisponibilidad = disponibilidades.Any(disponibilidad =>
                disponibilidad.IdDocente == horario.IdDocente &&
                MismoDia(disponibilidad.Dia, horario.FranjaHoraria.DiaSemana) &&
                disponibilidad.HoraInicio <= horario.FranjaHoraria.HoraInicio &&
                disponibilidad.HoraFin >= horario.FranjaHoraria.HoraFin);

            if (!tieneDisponibilidad)
            {
                AgregarConflicto(
                    conflictos,
                    "Docente sin disponibilidad",
                    "El horario está fuera de la disponibilidad registrada del docente.",
                    horario);
            }
        }
    }

    private static void DetectarDocenteNoDictaMateria(
        List<Horario> horarios,
        List<DocenteMateria> docenteMaterias,
        List<ReporteConflictoHorarioDto> conflictos)
    {
        foreach (var horario in horarios)
        {
            bool puedeDictar = docenteMaterias.Any(docenteMateria =>
                docenteMateria.IdDocente == horario.IdDocente &&
                docenteMateria.IdMateria == horario.IdMateria);

            if (!puedeDictar)
            {
                AgregarConflicto(
                    conflictos,
                    "Docente no asignado a materia",
                    "El docente no aparece asociado como docente habilitado para la materia.",
                    horario);
            }
        }
    }

    private static void DetectarMateriaFueraPlan(
        List<Horario> horarios,
        List<MateriaPlan> materiasPlan,
        List<ReporteConflictoHorarioDto> conflictos)
    {
        foreach (var horario in horarios)
        {
            if (horario.Grupo == null)
            {
                continue;
            }

            bool perteneceAlPlan = materiasPlan.Any(materiaPlan =>
                materiaPlan.IdMateria == horario.IdMateria &&
                materiaPlan.SemestrePlan != null &&
                materiaPlan.SemestrePlan.IdPlanAcademico == horario.Grupo.IdPlanAcademico &&
                materiaPlan.SemestrePlan.NumeroSemestre == horario.Grupo.NumeroSemestre);

            if (!perteneceAlPlan)
            {
                AgregarConflicto(
                    conflictos,
                    "Materia fuera del plan",
                    "La materia asignada no pertenece al plan académico y semestre del grupo.",
                    horario);
            }
        }
    }

    private static void AgregarConflicto(
        List<ReporteConflictoHorarioDto> conflictos,
        string tipo,
        string descripcion,
        Horario horario)
    {
        string grupo = horario.Grupo?.Codigo ?? string.Empty;
        string docente = horario.Docente?.NombreCompleto ?? string.Empty;
        string materia = horario.Materia?.Nombre ?? string.Empty;
        string dia = horario.FranjaHoraria?.DiaSemana ?? string.Empty;
        string inicio = horario.FranjaHoraria != null ? FormatearHora(horario.FranjaHoraria.HoraInicio) : string.Empty;
        string fin = horario.FranjaHoraria != null ? FormatearHora(horario.FranjaHoraria.HoraFin) : string.Empty;

        bool existe = conflictos.Any(conflicto =>
            conflicto.TipoConflicto == tipo &&
            conflicto.Grupo == grupo &&
            conflicto.Docente == docente &&
            conflicto.Materia == materia &&
            conflicto.DiaSemana == dia &&
            conflicto.HoraInicio == inicio &&
            conflicto.HoraFin == fin);

        if (existe)
        {
            return;
        }

        conflictos.Add(new ReporteConflictoHorarioDto
        {
            TipoConflicto = tipo,
            Descripcion = descripcion,
            Grupo = grupo,
            Docente = docente,
            Materia = materia,
            DiaSemana = dia,
            HoraInicio = inicio,
            HoraFin = fin
        });
    }

    private static bool HaySolape(
        TimeSpan inicioA,
        TimeSpan finA,
        TimeSpan inicioB,
        TimeSpan finB)
    {
        return inicioA < finB && inicioB < finA;
    }

    private static bool MismoDia(string diaA, string diaB)
    {
        return Normalizar(diaA) == Normalizar(diaB);
    }

    private static int OrdenDia(string dia)
    {
        return Normalizar(dia) switch
        {
            "lunes" => 1,
            "martes" => 2,
            "miercoles" => 3,
            "jueves" => 4,
            "viernes" => 5,
            "sabado" => 6,
            "domingo" => 7,
            _ => 99
        };
    }

    private static string FormatearHora(TimeSpan hora)
    {
        return hora.ToString(@"hh\:mm");
    }

    private static string ObtenerEstadoGrupo(List<string> estados)
    {
        var estadosDistintos = estados
            .Where(estado => !string.IsNullOrWhiteSpace(estado))
            .Distinct()
            .ToList();

        if (estadosDistintos.Count == 0)
        {
            return "Pendiente";
        }

        return estadosDistintos.Count == 1
            ? estadosDistintos[0]
            : "Mixto";
    }

    private static string Normalizar(string valor)
    {
        string texto = valor.Trim().ToLowerInvariant();
        string normalizado = texto.Normalize(NormalizationForm.FormD);

        var builder = new StringBuilder();

        foreach (char caracter in normalizado)
        {
            UnicodeCategory categoria = CharUnicodeInfo.GetUnicodeCategory(caracter);

            if (categoria != UnicodeCategory.NonSpacingMark)
            {
                builder.Append(caracter);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string UnirCsv(params string[] valores)
    {
        return string.Join(";", valores.Select(EscaparCsv));
    }

    private static string EscaparCsv(string valor)
    {
        string limpio = valor.Replace("\r", " ").Replace("\n", " ");

        if (limpio.Contains(';') || limpio.Contains('"'))
        {
            return $"\"{limpio.Replace("\"", "\"\"")}\"";
        }

        return limpio;
    }

    private static byte[] CrearCsvSimple(List<string> lineas)
    {
        string contenido = string.Join(Environment.NewLine, lineas);
        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(contenido)).ToArray();
    }

    private static byte[] CrearPdfSimple(List<string> lineas)
    {
        var textoPdf = new List<string>
        {
            "Reporte académico",
            $"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}",
            ""
        };

        foreach (string linea in lineas)
        {
            textoPdf.AddRange(DividirLineaParaPdf(linea, 95));
        }

        var contenido = new StringBuilder();
        contenido.AppendLine("BT");
        contenido.AppendLine("/F1 10 Tf");
        contenido.AppendLine("12 TL");
        contenido.AppendLine("50 790 Td");

        foreach (string linea in textoPdf.Take(58))
        {
            contenido.AppendLine($"({EscaparPdf(LimpiarTextoPdf(linea))}) Tj");
            contenido.AppendLine("T*");
        }

        contenido.AppendLine("ET");

        string stream = contenido.ToString();
        int longitud = Encoding.ASCII.GetByteCount(stream);

        var objetos = new List<string>
        {
            "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n",
            "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n",
            "3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>\nendobj\n",
            "4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>\nendobj\n",
            $"5 0 obj\n<< /Length {longitud} >>\nstream\n{stream}endstream\nendobj\n"
        };

        var pdf = new StringBuilder();
        pdf.AppendLine("%PDF-1.4");

        var offsets = new List<int> { 0 };

        foreach (string objeto in objetos)
        {
            offsets.Add(Encoding.ASCII.GetByteCount(pdf.ToString()));
            pdf.Append(objeto);
        }

        int inicioXref = Encoding.ASCII.GetByteCount(pdf.ToString());

        pdf.AppendLine("xref");
        pdf.AppendLine($"0 {objetos.Count + 1}");
        pdf.AppendLine("0000000000 65535 f ");

        foreach (int offset in offsets.Skip(1))
        {
            pdf.AppendLine($"{offset:0000000000} 00000 n ");
        }

        pdf.AppendLine("trailer");
        pdf.AppendLine($"<< /Size {objetos.Count + 1} /Root 1 0 R >>");
        pdf.AppendLine("startxref");
        pdf.AppendLine(inicioXref.ToString());
        pdf.AppendLine("%%EOF");

        return Encoding.ASCII.GetBytes(pdf.ToString());
    }

    private static List<string> DividirLineaParaPdf(string linea, int maximo)
    {
        var resultado = new List<string>();
        string restante = linea;

        while (restante.Length > maximo)
        {
            resultado.Add(restante[..maximo]);
            restante = restante[maximo..];
        }

        resultado.Add(restante);
        return resultado;
    }

    private static string LimpiarTextoPdf(string texto)
    {
        string normalizado = texto.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder();

        foreach (char caracter in normalizado)
        {
            UnicodeCategory categoria = CharUnicodeInfo.GetUnicodeCategory(caracter);

            if (categoria == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            builder.Append(caracter <= 127 ? caracter : ' ');
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static string EscaparPdf(string texto)
    {
        return texto
            .Replace("\\", "\\\\")
            .Replace("(", "\\(")
            .Replace(")", "\\)");
    }
}
