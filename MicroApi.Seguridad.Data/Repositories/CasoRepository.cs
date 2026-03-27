using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.Json;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Models.Soporte;
using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Security;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class CasoRepository : ICasoRepository
    {
        private readonly ApplicationDbContext _context;

        public CasoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Caso>> GetAllAsync()
        {
            return await _context.Casos
                .Include(c => c.EstadoCaso)
                .Include(c => c.TipoCaso)
                .Include(c => c.Prioridad)
                .Include(c => c.CanalIngreso)
                .Include(c => c.AreaTecnica)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.CategoriaActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.EstadoActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.Ubicacion)
                        .ThenInclude(u => u.Sede)
                .Include(c => c.UsuarioReporta)
                .Include(c => c.TecnicoAsignado)
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        public async Task<Caso?> GetByIdAsync(long id)
        {
            return await _context.Casos
                .Include(c => c.EstadoCaso)
                .Include(c => c.TipoCaso)
                .Include(c => c.Prioridad)
                .Include(c => c.CanalIngreso)
                .Include(c => c.AreaTecnica)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.CategoriaActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.EstadoActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.Ubicacion)
                        .ThenInclude(u => u.Sede)
                .Include(c => c.UsuarioReporta)
                .Include(c => c.TecnicoAsignado)
                .Include(c => c.Trazabilidades)
                    .ThenInclude(t => t.EstadoCaso)
                .Include(c => c.Trazabilidades)
                    .ThenInclude(t => t.AreaTecnica)
                .Include(c => c.Trazabilidades)
                    .ThenInclude(t => t.IntervencionesTecnicas)
                        .ThenInclude(i => i.TipoTrabajo)
                .Include(c => c.Trazabilidades)
                    .ThenInclude(t => t.IntervencionesTecnicas)
                        .ThenInclude(i => i.EstadoIntervencion)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Caso>> GetByActivoAsync(long idActivo)
        {
            return await _context.Casos
                .Include(c => c.EstadoCaso)
                .Include(c => c.TipoCaso)
                .Include(c => c.Prioridad)
                .Include(c => c.CanalIngreso)
                .Include(c => c.AreaTecnica)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.CategoriaActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.EstadoActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.Ubicacion)
                        .ThenInclude(u => u.Sede)
                .Include(c => c.UsuarioReporta)
                .Include(c => c.TecnicoAsignado)
                .Where(c => c.IdActivo == idActivo)
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Caso>> GetByTecnicoAsync(long idTecnico)
        {
            return await _context.Casos
                .Include(c => c.EstadoCaso)
                .Include(c => c.TipoCaso)
                .Include(c => c.Prioridad)
                .Include(c => c.CanalIngreso)
                .Include(c => c.AreaTecnica)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.CategoriaActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.EstadoActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.Ubicacion)
                        .ThenInclude(u => u.Sede)
                .Include(c => c.UsuarioReporta)
                .Include(c => c.TecnicoAsignado)
                .Where(c => c.IdTecnicoAsignado == idTecnico)
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Caso>> GetByUsuarioReportaAsync(long idUsuarioReporta)
        {
            return await _context.Casos
                .Include(c => c.EstadoCaso)
                .Include(c => c.TipoCaso)
                .Include(c => c.Prioridad)
                .Include(c => c.CanalIngreso)
                .Include(c => c.AreaTecnica)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.CategoriaActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.EstadoActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.Ubicacion)
                        .ThenInclude(u => u.Sede)
                .Include(c => c.UsuarioReporta)
                .Include(c => c.TecnicoAsignado)
                .Where(c => c.IdUsuarioReporta == idUsuarioReporta)
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Caso>> GetByEstadoAsync(long idEstadoCaso)
        {
            return await _context.Casos
                .Include(c => c.EstadoCaso)
                .Include(c => c.TipoCaso)
                .Include(c => c.Prioridad)
                .Include(c => c.CanalIngreso)
                .Include(c => c.AreaTecnica)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.CategoriaActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.EstadoActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.Ubicacion)
                        .ThenInclude(u => u.Sede)
                .Include(c => c.UsuarioReporta)
                .Include(c => c.TecnicoAsignado)
                .Where(c => c.IdEstadoCaso == idEstadoCaso)
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        public async Task<IEnumerable<Caso>> GetByFiltrosAsync(
            long? idEstadoCaso, 
            long? idTecnico, 
            long? idAreaTecnica, 
            DateTime? fechaDesde, 
            DateTime? fechaHasta)
        {
            var query = _context.Casos
                .Include(c => c.EstadoCaso)
                .Include(c => c.TipoCaso)
                .Include(c => c.Prioridad)
                .Include(c => c.CanalIngreso)
                .Include(c => c.AreaTecnica)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.CategoriaActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.EstadoActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.Ubicacion)
                        .ThenInclude(u => u.Sede)
                .Include(c => c.UsuarioReporta)
                .Include(c => c.TecnicoAsignado)
                .AsQueryable();

            if (idEstadoCaso.HasValue)
            {
                query = query.Where(c => c.IdEstadoCaso == idEstadoCaso.Value);
            }

            if (idTecnico.HasValue)
            {
                query = query.Where(c => c.IdTecnicoAsignado == idTecnico.Value);
            }

            if (idAreaTecnica.HasValue)
            {
                query = query.Where(c => c.IdAreaTecnica == idAreaTecnica.Value);
            }

            if (fechaDesde.HasValue)
            {
                query = query.Where(c => c.FechaRegistro >= fechaDesde.Value);
            }

            if (fechaHasta.HasValue)
            {
                query = query.Where(c => c.FechaRegistro <= fechaHasta.Value);
            }

            return await query.OrderByDescending(c => c.FechaRegistro).ToListAsync();
        }

        public async Task<Caso> CreateAsync(Caso caso)
        {
            _context.Casos.Add(caso);
            await _context.SaveChangesAsync();
            return caso;
        }

        public async Task<Caso> UpdateAsync(Caso caso)
        {
            caso.FechaActualizacion = DateTime.UtcNow;
            _context.Casos.Update(caso);
            await _context.SaveChangesAsync();
            return caso;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Casos.AnyAsync(c => c.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Casos.CountAsync();
        }

        public async Task<IEnumerable<Caso>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Casos
                .Include(c => c.EstadoCaso)
                .Include(c => c.TipoCaso)
                .Include(c => c.Prioridad)
                .Include(c => c.CanalIngreso)
                .Include(c => c.AreaTecnica)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.CategoriaActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.EstadoActivo)
                .Include(c => c.Activo)
                    .ThenInclude(a => a.Ubicacion)
                        .ThenInclude(u => u.Sede)
                .Include(c => c.UsuarioReporta)
                .Include(c => c.TecnicoAsignado)
                .OrderByDescending(c => c.FechaRegistro)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<TecnicoIncidenciasResumenDto>> GetResumenTecnicosAsync()
        {
            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .Include(usuario => usuario.Rol)
                .Select(usuario => new
                {
                    usuario.Id,
                    usuario.NombreCompleto,
                    usuario.Email,
                    NombreRol = usuario.Rol.NombreRol
                })
                .ToListAsync();

            var tecnicos = usuarios
                .Where(usuario =>
                {
                    var rol = AppRoles.Normalize(usuario.NombreRol);
                    return rol == AppRoles.Tecnico || rol == AppRoles.Administrador;
                })
                .ToList();

            if (tecnicos.Count == 0)
            {
                return Array.Empty<TecnicoIncidenciasResumenDto>();
            }

            var tecnicoIds = tecnicos.Select(tecnico => tecnico.Id).ToList();

            var casos = await _context.Casos
                .AsNoTracking()
                .Where(caso => caso.IdTecnicoAsignado.HasValue && tecnicoIds.Contains(caso.IdTecnicoAsignado.Value))
                .Select(caso => new CasoTecnicoMetricasProjection
                {
                    IdTecnicoAsignado = caso.IdTecnicoAsignado!.Value,
                    NombreEstadoCaso = caso.EstadoCaso.NombreEstadoCaso,
                    NombrePrioridad = caso.Prioridad.NombrePrioridad,
                    TiempoResolucionDias = caso.Prioridad.TiempoResolucionDias,
                    NombreAreaTecnica = caso.AreaTecnica != null ? caso.AreaTecnica.NombreAreaTecnica : null,
                    FechaRegistro = caso.FechaRegistro,
                    FechaResolucion = caso.FechaResolucion,
                    FechaCierre = caso.FechaCierre,
                    FechaActualizacion = caso.FechaActualizacion
                })
                .ToListAsync();

            var casosPorTecnico = casos
                .GroupBy(caso => caso.IdTecnicoAsignado)
                .ToDictionary(group => group.Key, group => group.ToList());

            var resumen = tecnicos
                .Select(tecnico =>
                {
                    var incidenciasAsignadas = casosPorTecnico.TryGetValue(tecnico.Id, out var items)
                        ? items
                        : new List<CasoTecnicoMetricasProjection>();

                    var activas = incidenciasAsignadas
                        .Where(incidencia => ClasificarEstado(incidencia.NombreEstadoCaso) != "Cerrado")
                        .ToList();

                    var pendientes = activas
                        .Where(incidencia => ClasificarEstado(incidencia.NombreEstadoCaso) == "Pendiente")
                        .ToList();

                    var enProceso = activas
                        .Where(incidencia => ClasificarEstado(incidencia.NombreEstadoCaso) == "En Proceso")
                        .ToList();

                    var criticas = activas
                        .Where(incidencia => MapPrioridad(incidencia.NombrePrioridad) == "Critica")
                        .ToList();

                    var altas = activas
                        .Where(incidencia => MapPrioridad(incidencia.NombrePrioridad) == "Alta")
                        .ToList();

                    var vencidas = activas
                        .Where(incidencia => MapCumplioSla(GetSlaStatus(
                            incidencia.NombreEstadoCaso,
                            CalcularDias(incidencia.FechaRegistro, incidencia.FechaResolucion ?? incidencia.FechaCierre),
                            incidencia.TiempoResolucionDias,
                            incidencia.NombrePrioridad
                        )) == false)
                        .ToList();

                    var cerradas = incidenciasAsignadas
                        .Where(incidencia => ClasificarEstado(incidencia.NombreEstadoCaso) == "Cerrado")
                        .ToList();

                    var cerradasConResolucion = cerradas
                        .Where(incidencia => incidencia.FechaResolucion.HasValue)
                        .ToList();

                    var slaEvaluable = incidenciasAsignadas
                        .Select(incidencia => MapCumplioSla(GetSlaStatus(
                            incidencia.NombreEstadoCaso,
                            CalcularDias(incidencia.FechaRegistro, incidencia.FechaResolucion ?? incidencia.FechaCierre),
                            incidencia.TiempoResolucionDias,
                            incidencia.NombrePrioridad
                        )))
                        .Where(resultado => resultado.HasValue)
                        .Select(resultado => resultado!.Value)
                        .ToList();

                    return new TecnicoIncidenciasResumenDto
                    {
                        Id = tecnico.Id,
                        Nombre = tecnico.NombreCompleto,
                        Email = tecnico.Email,
                        Area = ResolverArea(activas, incidenciasAsignadas),
                        IncidenciasAsignadas = incidenciasAsignadas.Count,
                        IncidenciasActivas = activas.Count,
                        IncidenciasPendientes = pendientes.Count,
                        IncidenciasEnProceso = enProceso.Count,
                        IncidenciasCriticas = criticas.Count,
                        IncidenciasAltaPrioridad = altas.Count,
                        IncidenciasVencidas = vencidas.Count,
                        IncidenciasCerradas = cerradas.Count,
                        PromedioResolucion = cerradasConResolucion.Count == 0
                            ? 0
                            : Redondear1(cerradasConResolucion.Average(incidencia =>
                                Math.Max((incidencia.FechaResolucion!.Value - incidencia.FechaRegistro).TotalHours, 0))),
                        CumplimientoSLA = slaEvaluable.Count == 0
                            ? 0
                            : Redondear1((slaEvaluable.Count(resultado => resultado) / (double)slaEvaluable.Count) * 100)
                    };
                })
                .OrderByDescending(tecnico => tecnico.IncidenciasActivas)
                .ThenByDescending(tecnico => tecnico.IncidenciasCriticas)
                .ThenBy(tecnico => tecnico.Nombre)
                .ToList();

            return resumen;
        }

        // ==================== STORED PROCEDURES ====================

        public async Task<dynamic> SpCasoCrearAsync(CasoCreateDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@Descripcion", dto.Descripcion),
                new SqlParameter("@IdUsuarioReporta", dto.IdUsuarioReporta),
                new SqlParameter("@TelefonoContacto", (object?)dto.TelefonoContacto ?? DBNull.Value),
                new SqlParameter("@CorreoContacto", (object?)dto.CorreoContacto ?? DBNull.Value),
                new SqlParameter("@IdTipoCaso", dto.IdTipoCaso),
                new SqlParameter("@IdPrioridad", dto.IdPrioridad),
                new SqlParameter("@IdCanalIngreso", dto.IdCanalIngreso),
                new SqlParameter("@IdAreaTecnica", (object?)dto.IdAreaTecnica ?? DBNull.Value),
                new SqlParameter("@IdActivo", (object?)dto.IdActivo ?? DBNull.Value),
                new SqlParameter("@IdTecnicoAsignado", (object?)dto.IdTecnicoAsignado ?? DBNull.Value),
                new SqlParameter("@IdUsuarioCreacion", dto.IdUsuarioCreacion),
                new SqlParameter("@IdCasoNuevo", SqlDbType.BigInt) { Direction = ParameterDirection.Output },
                new SqlParameter("@NumeroCaso", SqlDbType.VarChar, 50) { Direction = ParameterDirection.Output }
            };

            var result = await _context.Database
                .ExecuteSqlRawAsync("EXEC soporte.spCasoCrear @Descripcion, @IdUsuarioReporta, @TelefonoContacto, " +
                    "@CorreoContacto, @IdTipoCaso, @IdPrioridad, @IdCanalIngreso, @IdAreaTecnica, @IdActivo, " +
                    "@IdTecnicoAsignado, @IdUsuarioCreacion, @IdCasoNuevo OUTPUT, @NumeroCaso OUTPUT", parameters);

            var idCasoNuevo = (long)parameters[11].Value!;
            var numeroCaso = parameters[12].Value?.ToString();

            // Obtener el caso creado con relaciones
            var caso = await GetByIdAsync(idCasoNuevo);
            return caso!;
        }

        public async Task<dynamic> SpCasoCambiarEstadoAsync(CasoCambiarEstadoDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdCaso", dto.IdCaso),
                new SqlParameter("@IdEstadoNuevo", dto.IdEstadoNuevo),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion),
                new SqlParameter("@Comentario", (object?)dto.Comentario ?? DBNull.Value)
            };

            await _context.Database
                .ExecuteSqlRawAsync("EXEC soporte.spCasoCambiarEstado @IdCaso, @IdEstadoNuevo, @IdUsuarioAccion, @Comentario", parameters);

            // Obtener el caso actualizado
            var caso = await GetByIdAsync(dto.IdCaso);
            return caso!;
        }

        public async Task<dynamic> SpCasoAsignarTecnicoAsync(CasoAsignarTecnicoDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdCaso", dto.IdCaso),
                new SqlParameter("@IdTecnicoAsignado", dto.IdTecnicoAsignado),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion),
                new SqlParameter("@Comentario", (object?)dto.Comentario ?? DBNull.Value)
            };

            await _context.Database
                .ExecuteSqlRawAsync("EXEC soporte.spCasoAsignarTecnico @IdCaso, @IdTecnicoAsignado, @IdUsuarioAccion, @Comentario", parameters);

            var caso = await GetByIdAsync(dto.IdCaso);
            return caso!;
        }

        public async Task<dynamic> SpCasoEscalarAsync(CasoEscalarDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdCaso", dto.IdCaso),
                new SqlParameter("@IdAreaTecnicaNueva", dto.IdAreaTecnicaNueva),
                new SqlParameter("@IdTecnicoAsignadoNuevo", (object?)dto.IdTecnicoAsignadoNuevo ?? DBNull.Value),
                new SqlParameter("@MotivoEscalamiento", dto.MotivoEscalamiento),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion)
            };

            await _context.Database
                .ExecuteSqlRawAsync("EXEC soporte.spCasoEscalar @IdCaso, @IdAreaTecnicaNueva, @IdTecnicoAsignadoNuevo, @MotivoEscalamiento, @IdUsuarioAccion", parameters);

            var caso = await GetByIdAsync(dto.IdCaso);
            return caso!;
        }

        public async Task<dynamic> SpCasoAsignarActivoAsync(CasoAsignarActivoDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdCaso", dto.IdCaso),
                new SqlParameter("@IdActivo", dto.IdActivo),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion)
            };

            await _context.Database
                .ExecuteSqlRawAsync("EXEC soporte.spCasoAsignarActivo @IdCaso, @IdActivo, @IdUsuarioAccion", parameters);

            var caso = await GetByIdAsync(dto.IdCaso);
            return caso!;
        }

        public async Task<(int TotalRegistros, List<dynamic> Casos)> SpCasoObtenerPorFiltrosAsync(
            long? idEstadoCaso, long? idTecnicoAsignado, long? idAreaTecnica, long? idPrioridad,
            long? idUsuarioReporta, DateTime? fechaDesde, DateTime? fechaHasta, string? textoBusqueda,
            int page, int pageSize, string orderBy, string orderDirection)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdEstadoCaso", (object?)idEstadoCaso ?? DBNull.Value),
                new SqlParameter("@IdTecnicoAsignado", (object?)idTecnicoAsignado ?? DBNull.Value),
                new SqlParameter("@IdAreaTecnica", (object?)idAreaTecnica ?? DBNull.Value),
                new SqlParameter("@IdPrioridad", (object?)idPrioridad ?? DBNull.Value),
                new SqlParameter("@IdUsuarioReporta", (object?)idUsuarioReporta ?? DBNull.Value),
                new SqlParameter("@FechaDesde", (object?)fechaDesde ?? DBNull.Value),
                new SqlParameter("@FechaHasta", (object?)fechaHasta ?? DBNull.Value),
                new SqlParameter("@TextoBusqueda", (object?)textoBusqueda ?? DBNull.Value),
                new SqlParameter("@Page", page),
                new SqlParameter("@PageSize", pageSize),
                new SqlParameter("@OrderBy", orderBy),
                new SqlParameter("@OrderDirection", orderDirection)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spCasoObtenerPorFiltros";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            
            // Result Set 1: Total de registros
            int totalRegistros = 0;
            if (await reader.ReadAsync())
            {
                totalRegistros = reader.GetInt32(0);
            }

            // Result Set 2: Casos
            await reader.NextResultAsync();
            var casos = new List<dynamic>();
            while (await reader.ReadAsync())
            {
                casos.Add(new
                {
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                    Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                    IdUsuarioReporta = reader.GetInt64(reader.GetOrdinal("IdUsuarioReporta")),
                    NombreUsuarioReporta = reader.GetString(reader.GetOrdinal("NombreUsuarioReporta")),
                    TelefonoContacto = reader.IsDBNull(reader.GetOrdinal("TelefonoContacto")) ? null : reader.GetString(reader.GetOrdinal("TelefonoContacto")),
                    CorreoContacto = reader.IsDBNull(reader.GetOrdinal("CorreoContacto")) ? null : reader.GetString(reader.GetOrdinal("CorreoContacto")),
                    IdTecnicoAsignado = reader.IsDBNull(reader.GetOrdinal("IdTecnicoAsignado")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("IdTecnicoAsignado")),
                    NombreTecnicoAsignado = reader.IsDBNull(reader.GetOrdinal("NombreTecnicoAsignado")) ? null : reader.GetString(reader.GetOrdinal("NombreTecnicoAsignado")),
                    IdEstadoCaso = reader.GetInt64(reader.GetOrdinal("IdEstadoCaso")),
                    NombreEstadoCaso = reader.GetString(reader.GetOrdinal("NombreEstadoCaso")),
                    IdPrioridad = reader.GetInt64(reader.GetOrdinal("IdPrioridad")),
                    NombrePrioridad = reader.GetString(reader.GetOrdinal("NombrePrioridad")),
                    IdTipoCaso = reader.GetInt64(reader.GetOrdinal("IdTipoCaso")),
                    NombreTipoCaso = reader.GetString(reader.GetOrdinal("NombreTipoCaso")),
                    IdCanalIngreso = reader.GetInt64(reader.GetOrdinal("IdCanalIngreso")),
                    NombreCanal = reader.GetString(reader.GetOrdinal("NombreCanal")),
                    IdAreaTecnica = reader.IsDBNull(reader.GetOrdinal("IdAreaTecnica")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("IdAreaTecnica")),
                    NombreAreaTecnica = reader.IsDBNull(reader.GetOrdinal("NombreAreaTecnica")) ? null : reader.GetString(reader.GetOrdinal("NombreAreaTecnica")),
                    IdActivo = reader.IsDBNull(reader.GetOrdinal("IdActivo")) ? (long?)null : reader.GetInt64(reader.GetOrdinal("IdActivo")),
                    NombreActivo = reader.IsDBNull(reader.GetOrdinal("NombreActivo")) ? null : reader.GetString(reader.GetOrdinal("NombreActivo")),
                    FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                    FechaAceptacion = reader.IsDBNull(reader.GetOrdinal("FechaAceptacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAceptacion")),
                    FechaResolucion = reader.IsDBNull(reader.GetOrdinal("FechaResolucion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaResolucion")),
                    FechaCierre = reader.IsDBNull(reader.GetOrdinal("FechaCierre")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCierre")),
                    FechaActualizacion = reader.IsDBNull(reader.GetOrdinal("FechaActualizacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
                    HorasTranscurridas = reader.GetInt32(reader.GetOrdinal("HorasTranscurridas")),
                    EstadoSLA = reader.GetString(reader.GetOrdinal("EstadoSLA"))
                });
            }

            return (totalRegistros, casos);
        }

        public async Task<CasoHistorialCompletoDto> SpCasoObtenerHistorialAsync(long idCaso)
        {
            var parameter = new SqlParameter("@IdCaso", idCaso);

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spCasoObtenerHistorial";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(parameter);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            
            var historial = new CasoHistorialCompletoDto();

            // Result Set 1: Información del caso
            if (await reader.ReadAsync())
            {
                historial.Caso = new CasoDetalleCompletoDto
                {
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                    Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                    IdUsuarioReporta = reader.GetInt64(reader.GetOrdinal("IdUsuarioReporta")),
                    NombreUsuarioReporta = reader.GetString(reader.GetOrdinal("NombreUsuarioReporta")),
                    EmailUsuarioReporta = reader.IsDBNull(reader.GetOrdinal("EmailUsuarioReporta")) ? null : reader.GetString(reader.GetOrdinal("EmailUsuarioReporta")),
                    TelefonoContacto = reader.IsDBNull(reader.GetOrdinal("TelefonoContacto")) ? null : reader.GetString(reader.GetOrdinal("TelefonoContacto")),
                    CorreoContacto = reader.IsDBNull(reader.GetOrdinal("CorreoContacto")) ? null : reader.GetString(reader.GetOrdinal("CorreoContacto")),
                    IdTecnicoAsignado = reader.IsDBNull(reader.GetOrdinal("IdTecnicoAsignado")) ? null : reader.GetInt64(reader.GetOrdinal("IdTecnicoAsignado")),
                    NombreTecnicoAsignado = reader.IsDBNull(reader.GetOrdinal("NombreTecnicoAsignado")) ? null : reader.GetString(reader.GetOrdinal("NombreTecnicoAsignado")),
                    EmailTecnicoAsignado = reader.IsDBNull(reader.GetOrdinal("EmailTecnicoAsignado")) ? null : reader.GetString(reader.GetOrdinal("EmailTecnicoAsignado")),
                    IdEstadoCaso = reader.GetInt64(reader.GetOrdinal("IdEstadoCaso")),
                    NombreEstadoCaso = reader.GetString(reader.GetOrdinal("NombreEstadoCaso")),
                    IdPrioridad = reader.GetInt64(reader.GetOrdinal("IdPrioridad")),
                    NombrePrioridad = reader.GetString(reader.GetOrdinal("NombrePrioridad")),
                    IdTipoCaso = reader.GetInt64(reader.GetOrdinal("IdTipoCaso")),
                    NombreTipoCaso = reader.GetString(reader.GetOrdinal("NombreTipoCaso")),
                    IdCanalIngreso = reader.GetInt64(reader.GetOrdinal("IdCanalIngreso")),
                    NombreCanal = reader.GetString(reader.GetOrdinal("NombreCanal")),
                    IdAreaTecnica = reader.IsDBNull(reader.GetOrdinal("IdAreaTecnica")) ? null : reader.GetInt64(reader.GetOrdinal("IdAreaTecnica")),
                    NombreAreaTecnica = reader.IsDBNull(reader.GetOrdinal("NombreAreaTecnica")) ? null : reader.GetString(reader.GetOrdinal("NombreAreaTecnica")),
                    IdActivo = reader.IsDBNull(reader.GetOrdinal("IdActivo")) ? null : reader.GetInt64(reader.GetOrdinal("IdActivo")),
                    NombreActivo = reader.IsDBNull(reader.GetOrdinal("NombreActivo")) ? null : reader.GetString(reader.GetOrdinal("NombreActivo")),
                    SerialActivo = reader.IsDBNull(reader.GetOrdinal("SerialActivo")) ? null : reader.GetString(reader.GetOrdinal("SerialActivo")),
                    FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                    FechaAceptacion = reader.IsDBNull(reader.GetOrdinal("FechaAceptacion")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaAceptacion")),
                    FechaResolucion = reader.IsDBNull(reader.GetOrdinal("FechaResolucion")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaResolucion")),
                    FechaCierre = reader.IsDBNull(reader.GetOrdinal("FechaCierre")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaCierre")),
                    FechaActualizacion = reader.IsDBNull(reader.GetOrdinal("FechaActualizacion")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaActualizacion")),
                    IdUsuarioCreacion = reader.GetInt64(reader.GetOrdinal("IdUsuarioCreacion")),
                    NombreUsuarioCreacion = reader.GetString(reader.GetOrdinal("NombreUsuarioCreacion"))
                };
            }

            // Result Set 2: Trazabilidad
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                historial.Trazabilidad.Add(new TrazabilidadDto
                {
                    Id = reader.GetInt64(reader.GetOrdinal("IdTrazabilidadCaso")),
                    FechaEvento = reader.GetDateTime(reader.GetOrdinal("FechaEvento")),
                    TipoEvento = reader.GetString(reader.GetOrdinal("TipoEvento")),
                    Comentario = reader.GetString(reader.GetOrdinal("Comentario")),
                    IdUsuarioAccion = reader.GetInt64(reader.GetOrdinal("IdUsuarioAccion")),
                    NombreUsuarioAccion = reader.GetString(reader.GetOrdinal("NombreUsuarioAccion")),
                    EmailUsuarioAccion = reader.IsDBNull(reader.GetOrdinal("EmailUsuarioAccion")) ? null : reader.GetString(reader.GetOrdinal("EmailUsuarioAccion")),
                    IdEstadoCaso = reader.IsDBNull(reader.GetOrdinal("IdEstadoCaso")) ? null : reader.GetInt64(reader.GetOrdinal("IdEstadoCaso")),
                    NombreEstadoCaso = reader.IsDBNull(reader.GetOrdinal("NombreEstadoCaso")) ? null : reader.GetString(reader.GetOrdinal("NombreEstadoCaso")),
                    IdAreaTecnica = reader.IsDBNull(reader.GetOrdinal("IdAreaTecnica")) ? null : reader.GetInt64(reader.GetOrdinal("IdAreaTecnica")),
                    NombreAreaTecnica = reader.IsDBNull(reader.GetOrdinal("NombreAreaTecnica")) ? null : reader.GetString(reader.GetOrdinal("NombreAreaTecnica")),
                    IdTecnicoAsignado = reader.IsDBNull(reader.GetOrdinal("IdTecnicoAsignado")) ? null : reader.GetInt64(reader.GetOrdinal("IdTecnicoAsignado")),
                    NombreTecnicoAsignado = reader.IsDBNull(reader.GetOrdinal("NombreTecnicoAsignado")) ? null : reader.GetString(reader.GetOrdinal("NombreTecnicoAsignado"))
                });
            }

            // Result Set 3: Intervenciones técnicas
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                var idCasoIntervencion = HasColumn(reader, "IdCaso")
                    ? reader.GetInt64(reader.GetOrdinal("IdCaso"))
                    : historial.Caso.IdCaso;
                var fechaCreacionIntervencion = HasColumn(reader, "FechaCreacion") && !reader.IsDBNull(reader.GetOrdinal("FechaCreacion"))
                    ? reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                    : reader.GetDateTime(reader.GetOrdinal("FechaInicio"));

                historial.Intervenciones.Add(new IntervencionTecnicaDetalleDto
                {
                    Id = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    IdCaso = idCasoIntervencion,
                    IdTipoTrabajo = reader.GetInt64(reader.GetOrdinal("IdTipoTrabajo")),
                    NombreTipoTrabajo = reader.GetString(reader.GetOrdinal("NombreTipoTrabajo")),
                    IdEstadoIntervencion = reader.GetInt64(reader.GetOrdinal("IdEstadoIntervencion")),
                    NombreEstadoIntervencion = reader.GetString(reader.GetOrdinal("NombreEstadoIntervencion")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? null : reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    Diagnostico = reader.GetString(reader.GetOrdinal("Diagnostico")),
                    SolucionAplicada = reader.IsDBNull(reader.GetOrdinal("SolucionAplicada")) ? null : reader.GetString(reader.GetOrdinal("SolucionAplicada")),
                    IdUsuarioAccion = reader.GetInt64(reader.GetOrdinal("IdUsuarioAccion")),
                    NombreTecnico = reader.GetString(reader.GetOrdinal("NombreTecnico")),
                    FechaCreacion = fechaCreacionIntervencion,
                    CantidadComponentes = reader.GetInt32(reader.GetOrdinal("CantidadComponentes")),
                    CantidadConsumibles = reader.GetInt32(reader.GetOrdinal("CantidadConsumibles"))
                });
            }

            // Result Set 4: Componentes
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                historial.Componentes.Add(new ComponenteUsadoDto
                {
                    Id = reader.GetInt64(reader.GetOrdinal("IdCambioComponente")),
                    IdIntervencionTecnica = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    IdComponente = reader.GetInt64(reader.GetOrdinal("IdComponente")),
                    NombreComponente = reader.GetString(reader.GetOrdinal("NombreComponente")),
                    Marca = reader.IsDBNull(reader.GetOrdinal("Marca")) ? null : reader.GetString(reader.GetOrdinal("Marca")),
                    Modelo = reader.IsDBNull(reader.GetOrdinal("Modelo")) ? null : reader.GetString(reader.GetOrdinal("Modelo")),
                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                    TipoCambio = reader.GetString(reader.GetOrdinal("TipoCambio")),
                    Descripcion = reader.IsDBNull(reader.GetOrdinal("DescripcionCambio")) ? null : reader.GetString(reader.GetOrdinal("DescripcionCambio")),
                    FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                });
            }

            // Result Set 5: Consumibles
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                historial.Consumibles.Add(new ConsumibleUsadoDto
                {
                    Id = reader.GetInt64(reader.GetOrdinal("IdDetalleConsumible")),
                    IdIntervencionTecnica = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    IdConsumible = reader.GetInt64(reader.GetOrdinal("IdConsumible")),
                    NombreConsumible = reader.GetString(reader.GetOrdinal("NombreConsumible")),
                    Marca = reader.IsDBNull(reader.GetOrdinal("Marca")) ? null : reader.GetString(reader.GetOrdinal("Marca")),
                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                    DescripcionUso = reader.IsDBNull(reader.GetOrdinal("DescripcionUso")) ? null : reader.GetString(reader.GetOrdinal("DescripcionUso")),
                    FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                });
            }

            // Result Set 6: Revisiones
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                historial.Revisiones.Add(new RevisionAdmiDetalleDto
                {
                    Id = reader.GetInt64(reader.GetOrdinal("IdRevisionAdmi")),
                    IdIntervencionTecnica = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    EstadoAprobacion = reader.GetBoolean(reader.GetOrdinal("Aprobado")) ? "APROBADO" : "RECHAZADO",
                    TipoRevision = reader.IsDBNull(reader.GetOrdinal("TipoRevision")) ? RevisionTipos.Diagnostico : reader.GetString(reader.GetOrdinal("TipoRevision")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("ObservacionRevision")) ? null : reader.GetString(reader.GetOrdinal("ObservacionRevision")),
                    FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                    IdUsuarioCreacion = reader.GetInt64(reader.GetOrdinal("IdUsuarioCreacion")),
                    NombreRevisor = reader.GetString(reader.GetOrdinal("NombreRevisor"))
                });
            }

            // Result Set 7: Encuestas
            await reader.NextResultAsync();
            while (await reader.ReadAsync())
            {
                historial.Encuestas.Add(new EncuestaCalidadDetalleDto
                {
                    Id = reader.GetInt64(reader.GetOrdinal("IdEncuesta")),
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    FechaEncuesta = reader.GetDateTime(reader.GetOrdinal("FechaEncuesta")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? null : reader.GetString(reader.GetOrdinal("Observaciones")),
                    IdUsuarioCreacion = reader.GetInt64(reader.GetOrdinal("IdUsuarioCreacion")),
                    NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                    CantidadRespuestas = reader.GetInt32(reader.GetOrdinal("CantidadRespuestas"))
                });
            }

            return historial;
        }

        private static bool HasColumn(DbDataReader reader, string columnName)
        {
            for (var index = 0; index < reader.FieldCount; index++)
            {
                if (string.Equals(reader.GetName(index), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ClasificarEstado(string? nombreEstadoCaso)
        {
            var estado = NormalizarTexto(nombreEstadoCaso);

            if (estado.Contains("cerrad") || estado.Contains("resuelt")) return "Cerrado";
            if (estado.Contains("escal")) return "Escalado";
            if (estado.Contains("asign") || estado.Contains("progreso") || estado.Contains("proceso")) return "En Proceso";
            return "Pendiente";
        }

        private static string MapPrioridad(string? nombrePrioridad)
        {
            var prioridad = NormalizarTexto(nombrePrioridad);

            if (prioridad.Contains("critic")) return "Critica";
            if (prioridad.Contains("alta")) return "Alta";
            if (prioridad.Contains("media")) return "Media";
            if (prioridad.Contains("baja")) return "Baja";
            return string.IsNullOrWhiteSpace(nombrePrioridad) ? "Media" : nombrePrioridad;
        }

        private static int ResolverLimiteSla(int tiempoResolucionDias, string? nombrePrioridad)
        {
            if (tiempoResolucionDias > 0)
            {
                return tiempoResolucionDias;
            }

            return MapPrioridad(nombrePrioridad) switch
            {
                "Critica" => 1,
                "Alta" => 2,
                "Media" => 4,
                "Baja" => 7,
                _ => 4,
            };
        }

        private static string GetSlaStatus(string? nombreEstadoCaso, int diasAbierto, int tiempoResolucionDias, string? nombrePrioridad)
        {
            var estado = ClasificarEstado(nombreEstadoCaso);
            if (estado == "Cerrado") return "Cumplido";

            var limite = ResolverLimiteSla(tiempoResolucionDias, nombrePrioridad);

            if (diasAbierto > limite) return "Vencido";
            if (diasAbierto == limite) return "En Riesgo";
            return "En Tiempo";
        }

        private static bool? MapCumplioSla(string? slaStatus)
        {
            var status = NormalizarTexto(slaStatus);

            if (string.IsNullOrWhiteSpace(status)) return null;
            if (status.Contains("venc") || status.Contains("incumpl")) return false;
            if (status.Contains("cumpl") || status.Contains("en tiempo") || status.Contains("ok")) return true;
            return null;
        }

        private static int CalcularDias(DateTime fechaInicio, DateTime? fechaFin)
        {
            var destino = fechaFin ?? DateTime.UtcNow;
            var diferencia = (int)Math.Ceiling((destino - fechaInicio).TotalDays);
            return Math.Max(diferencia, 0);
        }

        private static double Redondear1(double valor)
            => Math.Round(valor, 1, MidpointRounding.AwayFromZero);

        private static string ResolverArea(
            IReadOnlyCollection<CasoTecnicoMetricasProjection> activas,
            IReadOnlyCollection<CasoTecnicoMetricasProjection> incidenciasAsignadas)
        {
            var fuente = activas.Count > 0 ? activas : incidenciasAsignadas;

            var area = fuente
                .Where(incidencia => !string.IsNullOrWhiteSpace(incidencia.NombreAreaTecnica))
                .GroupBy(incidencia => incidencia.NombreAreaTecnica!)
                .Select(group => new
                {
                    Area = group.Key,
                    Cantidad = group.Count(),
                    UltimoMovimiento = group.Max(incidencia =>
                        incidencia.FechaActualizacion ??
                        incidencia.FechaResolucion ??
                        incidencia.FechaCierre ??
                        incidencia.FechaRegistro)
                })
                .OrderByDescending(group => group.Cantidad)
                .ThenByDescending(group => group.UltimoMovimiento)
                .Select(group => group.Area)
                .FirstOrDefault();

            return area ?? "No definida";
        }

        private static string NormalizarTexto(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            var normalized = value
                .Normalize(NormalizationForm.FormD)
                .Where(character => CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                .ToArray();

            return new string(normalized).Normalize(NormalizationForm.FormC).ToLowerInvariant().Trim();
        }

        private sealed class CasoTecnicoMetricasProjection
        {
            public long IdTecnicoAsignado { get; set; }
            public string? NombreEstadoCaso { get; set; }
            public string? NombrePrioridad { get; set; }
            public int TiempoResolucionDias { get; set; }
            public string? NombreAreaTecnica { get; set; }
            public DateTime FechaRegistro { get; set; }
            public DateTime? FechaResolucion { get; set; }
            public DateTime? FechaCierre { get; set; }
            public DateTime? FechaActualizacion { get; set; }
        }
    }
}
