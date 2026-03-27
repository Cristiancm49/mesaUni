using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class IntervencionTecnicaRepositoryExtended : IIntervencionTecnicaRepositoryExtended
    {
        private readonly ApplicationDbContext _context;

        public IntervencionTecnicaRepositoryExtended(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<dynamic> SpIntervencionTecnicaCrearAsync(IntervencionTecnicaCreateDto dto)
        {
            // Serializar componentes y consumibles a JSON
            string? componentesJson = dto.Componentes != null && dto.Componentes.Any() 
                ? JsonSerializer.Serialize(dto.Componentes) 
                : null;
            
            string? consumiblesJson = dto.Consumibles != null && dto.Consumibles.Any() 
                ? JsonSerializer.Serialize(dto.Consumibles) 
                : null;

            var parameters = new[]
            {
                new SqlParameter("@IdCaso", dto.IdCaso),
                new SqlParameter("@IdTipoTrabajo", dto.IdTipoTrabajo),
                new SqlParameter("@IdEstadoIntervencion", dto.IdEstadoIntervencion),
                new SqlParameter("@FechaInicio", dto.FechaInicio),
                new SqlParameter("@FechaFin", (object?)dto.FechaFin ?? DBNull.Value),
                new SqlParameter("@Diagnostico", dto.Diagnostico),
                new SqlParameter("@SolucionAplicada", (object?)dto.SolucionAplicada ?? DBNull.Value),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion),
                new SqlParameter("@ComponentesJSON", (object?)componentesJson ?? DBNull.Value),
                new SqlParameter("@ConsumiblesJSON", (object?)consumiblesJson ?? DBNull.Value),
                new SqlParameter("@IdIntervencionNueva", SqlDbType.BigInt) { Direction = ParameterDirection.Output }
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spIntervencionTecnicaCrear";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            
            dynamic? intervencion = null;
            List<dynamic> componentes = new();
            List<dynamic> consumibles = new();

            // Result Set 1: Intervención creada
            if (await reader.ReadAsync())
            {
                intervencion = new
                {
                    Id = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    IdTrazabilidadCaso = reader.GetInt64(reader.GetOrdinal("IdTrazabilidadCaso")),
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                    IdTipoTrabajo = reader.GetInt64(reader.GetOrdinal("IdTipoTrabajo")),
                    NombreTipoTrabajo = reader.GetString(reader.GetOrdinal("NombreTipoTrabajo")),
                    IdEstadoIntervencion = reader.GetInt64(reader.GetOrdinal("IdEstadoIntervencion")),
                    NombreEstadoIntervencion = reader.GetString(reader.GetOrdinal("NombreEstadoIntervencion")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    Diagnostico = reader.GetString(reader.GetOrdinal("Diagnostico")),
                    SolucionAplicada = reader.IsDBNull(reader.GetOrdinal("SolucionAplicada")) ? null : reader.GetString(reader.GetOrdinal("SolucionAplicada")),
                    IdUsuarioAccion = reader.GetInt64(reader.GetOrdinal("IdUsuarioAccion")),
                    NombreTecnico = reader.GetString(reader.GetOrdinal("NombreTecnico")),
                    FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                };
            }

            // Result Set 2: Componentes usados
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    componentes.Add(new
                    {
                        IdCambioComponente = reader.GetInt64(reader.GetOrdinal("IdCambioComponente")),
                        IdComponente = reader.GetInt64(reader.GetOrdinal("IdComponente")),
                        NombreComponente = reader.GetString(reader.GetOrdinal("NombreComponente")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                        TipoCambio = reader.GetString(reader.GetOrdinal("TipoCambio")),
                        DescripcionCambio = reader.IsDBNull(reader.GetOrdinal("DescripcionCambio")) ? null : reader.GetString(reader.GetOrdinal("DescripcionCambio"))
                    });
                }
            }

            // Result Set 3: Consumibles usados
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    consumibles.Add(new
                    {
                        IdDetalleConsumible = reader.GetInt64(reader.GetOrdinal("IdDetalleConsumible")),
                        IdConsumible = reader.GetInt64(reader.GetOrdinal("IdConsumible")),
                        NombreConsumible = reader.GetString(reader.GetOrdinal("NombreConsumible")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                        DescripcionUso = reader.IsDBNull(reader.GetOrdinal("DescripcionUso")) ? null : reader.GetString(reader.GetOrdinal("DescripcionUso"))
                    });
                }
            }

            return new
            {
                Intervencion = intervencion,
                Componentes = componentes,
                Consumibles = consumibles
            };
        }

        public async Task<dynamic> SpIntervencionTecnicaActualizarAsync(IntervencionTecnicaUpdateDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdIntervencionTecnica", dto.IdIntervencionTecnica),
                new SqlParameter("@IdTipoTrabajo", (object?)dto.IdTipoTrabajo ?? DBNull.Value),
                new SqlParameter("@IdEstadoIntervencion", (object?)dto.IdEstadoIntervencion ?? DBNull.Value),
                new SqlParameter("@FechaInicio", (object?)dto.FechaInicio ?? DBNull.Value),
                new SqlParameter("@FechaFin", (object?)dto.FechaFin ?? DBNull.Value),
                new SqlParameter("@Diagnostico", (object?)dto.Diagnostico ?? DBNull.Value),
                new SqlParameter("@SolucionAplicada", (object?)dto.SolucionAplicada ?? DBNull.Value),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spIntervencionTecnicaActualizar";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            
            // Leer intervención actualizada
            if (await reader.ReadAsync())
            {
                return new
                {
                    Id = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    IdTrazabilidadCaso = reader.GetInt64(reader.GetOrdinal("IdTrazabilidadCaso")),
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                    IdTipoTrabajo = reader.GetInt64(reader.GetOrdinal("IdTipoTrabajo")),
                    NombreTipoTrabajo = reader.GetString(reader.GetOrdinal("NombreTipoTrabajo")),
                    IdEstadoIntervencion = reader.GetInt64(reader.GetOrdinal("IdEstadoIntervencion")),
                    NombreEstadoIntervencion = reader.GetString(reader.GetOrdinal("NombreEstadoIntervencion")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    Diagnostico = reader.GetString(reader.GetOrdinal("Diagnostico")),
                    SolucionAplicada = reader.IsDBNull(reader.GetOrdinal("SolucionAplicada")) ? null : reader.GetString(reader.GetOrdinal("SolucionAplicada")),
                    IdUsuarioAccion = reader.GetInt64(reader.GetOrdinal("IdUsuarioAccion")),
                    NombreTecnico = reader.GetString(reader.GetOrdinal("NombreTecnico")),
                    FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                };
            }

            return null;
        }

        // ==================== NUEVOS MÉTODOS: FLUJO EN 3 FASES ====================

        public async Task<dynamic> SpIntervencionTecnicaCrearConDiagnosticoAsync(IntervencionDiagnosticoCreateDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdCaso", dto.IdCaso),
                new SqlParameter("@IdTipoTrabajo", dto.IdTipoTrabajo),
                new SqlParameter("@Diagnostico", dto.Diagnostico),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion),
                new SqlParameter("@IdIntervencionNueva", SqlDbType.BigInt) { Direction = ParameterDirection.Output }
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spIntervencionTecnicaCrearConDiagnostico";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            if (command.Connection.State != ConnectionState.Open)
                await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            
            // Leer intervención creada
            if (await reader.ReadAsync())
            {
                var result = new
                {
                    IdIntervencionTecnica = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    IdTrazabilidadCaso = reader.GetInt64(reader.GetOrdinal("IdTrazabilidadCaso")),
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                    IdTipoTrabajo = reader.GetInt64(reader.GetOrdinal("IdTipoTrabajo")),
                    NombreTipoTrabajo = reader.GetString(reader.GetOrdinal("NombreTipoTrabajo")),
                    IdEstadoIntervencion = reader.GetInt64(reader.GetOrdinal("IdEstadoIntervencion")),
                    NombreEstadoIntervencion = reader.GetString(reader.GetOrdinal("NombreEstadoIntervencion")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    Diagnostico = reader.GetString(reader.GetOrdinal("Diagnostico")),
                    SolucionAplicada = reader.IsDBNull(reader.GetOrdinal("SolucionAplicada")) ? null : reader.GetString(reader.GetOrdinal("SolucionAplicada")),
                    IdUsuarioAccion = reader.GetInt64(reader.GetOrdinal("IdUsuarioAccion")),
                    NombreTecnico = reader.GetString(reader.GetOrdinal("NombreTecnico"))
                };
                
                return result;
            }

            throw new Exception("No se pudo crear el diagnóstico. El SP no devolvió resultados.");
        }

        public async Task<dynamic> SpIntervencionTecnicaActualizarDiagnosticoAsync(IntervencionDiagnosticoUpdateDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdIntervencionTecnica", dto.IdIntervencionTecnica),
                new SqlParameter("@IdTipoTrabajo", (object?)dto.IdTipoTrabajo ?? DBNull.Value),
                new SqlParameter("@Diagnostico", dto.Diagnostico),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spIntervencionTecnicaActualizarDiagnostico";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            if (command.Connection.State != ConnectionState.Open)
                await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            
            // Leer intervención actualizada
            if (await reader.ReadAsync())
            {
                var result = new
                {
                    IdIntervencionTecnica = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    IdTrazabilidadCaso = reader.GetInt64(reader.GetOrdinal("IdTrazabilidadCaso")),
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                    IdTipoTrabajo = reader.GetInt64(reader.GetOrdinal("IdTipoTrabajo")),
                    NombreTipoTrabajo = reader.GetString(reader.GetOrdinal("NombreTipoTrabajo")),
                    IdEstadoIntervencion = reader.GetInt64(reader.GetOrdinal("IdEstadoIntervencion")),
                    NombreEstadoIntervencion = reader.GetString(reader.GetOrdinal("NombreEstadoIntervencion")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    Diagnostico = reader.GetString(reader.GetOrdinal("Diagnostico")),
                    SolucionAplicada = reader.IsDBNull(reader.GetOrdinal("SolucionAplicada")) ? null : reader.GetString(reader.GetOrdinal("SolucionAplicada")),
                    IdUsuarioAccion = reader.GetInt64(reader.GetOrdinal("IdUsuarioAccion")),
                    NombreTecnico = reader.GetString(reader.GetOrdinal("NombreTecnico"))
                };
                
                return result;
            }

            throw new Exception("No se pudo actualizar el diagnóstico.");
        }

        public async Task<dynamic> SpIntervencionTecnicaEjecutarAsync(IntervencionEjecutarDto dto)
        {
            // Serializar componentes y consumibles a JSON
            string? componentesJson = dto.Componentes != null && dto.Componentes.Any() 
                ? JsonSerializer.Serialize(dto.Componentes) 
                : null;
            
            string? consumiblesJson = dto.Consumibles != null && dto.Consumibles.Any() 
                ? JsonSerializer.Serialize(dto.Consumibles) 
                : null;

            var parameters = new[]
            {
                new SqlParameter("@IdIntervencionTecnica", dto.IdIntervencionTecnica),
                new SqlParameter("@SolucionAplicada", dto.SolucionAplicada),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion),
                new SqlParameter("@ComponentesJSON", (object?)componentesJson ?? DBNull.Value),
                new SqlParameter("@ConsumiblesJSON", (object?)consumiblesJson ?? DBNull.Value)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spIntervencionTecnicaEjecutar";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            
            dynamic? intervencion = null;
            List<dynamic> componentes = new();
            List<dynamic> consumibles = new();

            // Result Set 1: Intervención completada
            if (await reader.ReadAsync())
            {
                intervencion = new
                {
                    IdIntervencionTecnica = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    IdTrazabilidadCaso = reader.GetInt64(reader.GetOrdinal("IdTrazabilidadCaso")),
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                    IdTipoTrabajo = reader.GetInt64(reader.GetOrdinal("IdTipoTrabajo")),
                    NombreTipoTrabajo = reader.GetString(reader.GetOrdinal("NombreTipoTrabajo")),
                    IdEstadoIntervencion = reader.GetInt64(reader.GetOrdinal("IdEstadoIntervencion")),
                    NombreEstadoIntervencion = reader.GetString(reader.GetOrdinal("NombreEstadoIntervencion")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    Diagnostico = reader.GetString(reader.GetOrdinal("Diagnostico")),
                    SolucionAplicada = reader.GetString(reader.GetOrdinal("SolucionAplicada")),
                    IdUsuarioAccion = reader.GetInt64(reader.GetOrdinal("IdUsuarioAccion")),
                    NombreTecnico = reader.GetString(reader.GetOrdinal("NombreTecnico"))
                };
            }

            // Result Set 2: Componentes usados
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    componentes.Add(new
                    {
                        IdCambioComponente = reader.GetInt64(reader.GetOrdinal("IdCambioComponente")),
                        IdComponente = reader.GetInt64(reader.GetOrdinal("IdComponente")),
                        NombreComponente = reader.GetString(reader.GetOrdinal("NombreComponente")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                        TipoCambio = reader.GetString(reader.GetOrdinal("TipoCambio")),
                        DescripcionCambio = reader.IsDBNull(reader.GetOrdinal("DescripcionCambio")) ? null : reader.GetString(reader.GetOrdinal("DescripcionCambio"))
                    });
                }
            }

            // Result Set 3: Consumibles usados
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    consumibles.Add(new
                    {
                        IdDetalleConsumible = reader.GetInt64(reader.GetOrdinal("IdDetalleConsumible")),
                        IdConsumible = reader.GetInt64(reader.GetOrdinal("IdConsumible")),
                        NombreConsumible = reader.GetString(reader.GetOrdinal("NombreConsumible")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                        DescripcionUso = reader.IsDBNull(reader.GetOrdinal("DescripcionUso")) ? null : reader.GetString(reader.GetOrdinal("DescripcionUso"))
                    });
                }
            }

            return new
            {
                Intervencion = intervencion,
                Componentes = componentes,
                Consumibles = consumibles
            };
        }

        public async Task<dynamic> SpIntervencionTecnicaActualizarSolucionAsync(IntervencionSolucionUpdateDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdIntervencionTecnica", dto.IdIntervencionTecnica),
                new SqlParameter("@SolucionAplicada", dto.SolucionAplicada),
                new SqlParameter("@IdUsuarioAccion", dto.IdUsuarioAccion)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spIntervencionTecnicaActualizarSolucion";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            if (command.Connection.State != ConnectionState.Open)
                await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new
                {
                    IdIntervencionTecnica = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    IdTrazabilidadCaso = reader.GetInt64(reader.GetOrdinal("IdTrazabilidadCaso")),
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                    IdTipoTrabajo = reader.GetInt64(reader.GetOrdinal("IdTipoTrabajo")),
                    NombreTipoTrabajo = reader.GetString(reader.GetOrdinal("NombreTipoTrabajo")),
                    IdEstadoIntervencion = reader.GetInt64(reader.GetOrdinal("IdEstadoIntervencion")),
                    NombreEstadoIntervencion = reader.GetString(reader.GetOrdinal("NombreEstadoIntervencion")),
                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                    FechaFin = reader.IsDBNull(reader.GetOrdinal("FechaFin")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFin")),
                    Diagnostico = reader.GetString(reader.GetOrdinal("Diagnostico")),
                    SolucionAplicada = reader.IsDBNull(reader.GetOrdinal("SolucionAplicada")) ? null : reader.GetString(reader.GetOrdinal("SolucionAplicada")),
                    IdUsuarioAccion = reader.GetInt64(reader.GetOrdinal("IdUsuarioAccion")),
                    NombreTecnico = reader.GetString(reader.GetOrdinal("NombreTecnico"))
                };
            }

            throw new Exception("No se pudo actualizar la solucion.");
        }
    }
}
