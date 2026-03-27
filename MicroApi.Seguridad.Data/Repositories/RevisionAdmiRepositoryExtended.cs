using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MicroApi.Seguridad.Domain.DTOs.Soporte;
using MicroApi.Seguridad.Domain.Interfaces;
using System.Data;
using System.Data.Common;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class RevisionAdmiRepositoryExtended : IRevisionAdmiRepositoryExtended
    {
        private readonly ApplicationDbContext _context;

        public RevisionAdmiRepositoryExtended(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RevisionAdmiResponseDto> SpRevisionAdmiProcesarAsync(RevisionAdmiCreateDto dto)
        {
            var parameters = new[]
            {
                new SqlParameter("@IdIntervencionTecnica", dto.IdIntervencionTecnica),
                new SqlParameter("@Aprobado", dto.Aprobado),
                new SqlParameter("@TipoRevision", dto.TipoRevision),
                new SqlParameter("@ObservacionRevision", (object?)dto.ObservacionRevision ?? DBNull.Value),
                new SqlParameter("@IdUsuarioCreacion", dto.IdUsuarioCreacion),
                new SqlParameter("@IdRevisionNueva", SqlDbType.BigInt) { Direction = ParameterDirection.Output }
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spRevisionAdmiProcesar";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            if (command.Connection?.State != ConnectionState.Open)
                await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();

            RevisionAdmiResponseDto? revision = null;

            if (await reader.ReadAsync())
            {
                var aprobado = reader.GetBoolean(reader.GetOrdinal("Aprobado"));
                revision = new RevisionAdmiResponseDto
                {
                    Id = reader.GetInt64(reader.GetOrdinal("IdRevisionAdmi")),
                    IdIntervencionTecnica = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                    EstadoAprobacion = aprobado ? "APROBADO" : "RECHAZADO",
                    TipoRevision = reader.GetString(reader.GetOrdinal("TipoRevision")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("ObservacionRevision")) ? null : reader.GetString(reader.GetOrdinal("ObservacionRevision")),
                    FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                    IdUsuarioCreacion = reader.GetInt64(reader.GetOrdinal("IdUsuarioCreacion")),
                    NombreRevisor = reader.GetString(reader.GetOrdinal("NombreRevisor")),
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso"))
                };
            }

            return revision ?? throw new InvalidOperationException("No se pudo crear la revision administrativa");
        }

        public async Task<RevisionAdmiQueueDto> SpRevisionAdmiObtenerBandejaAsync()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spRevisionAdmiObtenerBandeja";
            command.CommandType = CommandType.StoredProcedure;

            if (command.Connection?.State != ConnectionState.Open)
                await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();

            var result = new RevisionAdmiQueueDto();

            while (await reader.ReadAsync())
            {
                result.PendientesDiagnostico.Add(MapQueueItem(reader));
            }

            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    result.PendientesSolucion.Add(MapQueueItem(reader));
                }
            }

            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    result.Historico.Add(MapQueueItem(reader));
                }
            }

            return result;
        }

        private static RevisionAdmiQueueItemDto MapQueueItem(DbDataReader reader)
        {
            var aprobadoOrdinal = reader.GetOrdinal("Aprobado");
            var revisionOrdinal = reader.GetOrdinal("IdRevisionAdmi");
            var fechaRevisionOrdinal = reader.GetOrdinal("FechaRevision");
            var observacionOrdinal = reader.GetOrdinal("ObservacionRevision");
            var revisorOrdinal = reader.GetOrdinal("NombreRevisor");
            var solucionOrdinal = reader.GetOrdinal("SolucionAplicada");
            var activoOrdinal = reader.GetOrdinal("NombreActivo");
            var tecnicoOrdinal = reader.GetOrdinal("NombreTecnicoAsignado");
            var areaOrdinal = reader.GetOrdinal("NombreAreaTecnica");
            var usuarioOrdinal = reader.GetOrdinal("NombreUsuarioReporta");
            var fechaFinOrdinal = reader.GetOrdinal("FechaFin");
            var diagnosticoOrdinal = reader.GetOrdinal("Diagnostico");

            return new RevisionAdmiQueueItemDto
            {
                IdIntervencionTecnica = reader.GetInt64(reader.GetOrdinal("IdIntervencionTecnica")),
                IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                DescripcionCaso = reader.GetString(reader.GetOrdinal("DescripcionCaso")),
                TipoRevision = reader.GetString(reader.GetOrdinal("TipoRevision")),
                NombreTipoTrabajo = reader.GetString(reader.GetOrdinal("NombreTipoTrabajo")),
                NombreEstadoIntervencion = reader.GetString(reader.GetOrdinal("NombreEstadoIntervencion")),
                NombreEstadoCaso = reader.GetString(reader.GetOrdinal("NombreEstadoCaso")),
                NombrePrioridad = reader.GetString(reader.GetOrdinal("NombrePrioridad")),
                NombreTecnicoAsignado = reader.IsDBNull(tecnicoOrdinal) ? null : reader.GetString(tecnicoOrdinal),
                NombreAreaTecnica = reader.IsDBNull(areaOrdinal) ? null : reader.GetString(areaOrdinal),
                NombreUsuarioReporta = reader.IsDBNull(usuarioOrdinal) ? null : reader.GetString(usuarioOrdinal),
                NombreActivo = reader.IsDBNull(activoOrdinal) ? null : reader.GetString(activoOrdinal),
                FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                FechaFin = reader.IsDBNull(fechaFinOrdinal) ? null : reader.GetDateTime(fechaFinOrdinal),
                Diagnostico = reader.IsDBNull(diagnosticoOrdinal) ? null : reader.GetString(diagnosticoOrdinal),
                SolucionAplicada = reader.IsDBNull(solucionOrdinal) ? null : reader.GetString(solucionOrdinal),
                CantidadComponentes = reader.GetInt64(reader.GetOrdinal("CantidadComponentes")),
                CantidadConsumibles = reader.GetInt64(reader.GetOrdinal("CantidadConsumibles")),
                IdRevisionAdmi = reader.IsDBNull(revisionOrdinal) ? null : reader.GetInt64(revisionOrdinal),
                Aprobado = reader.IsDBNull(aprobadoOrdinal) ? null : reader.GetBoolean(aprobadoOrdinal),
                EstadoAprobacion = reader.IsDBNull(reader.GetOrdinal("EstadoAprobacion")) ? null : reader.GetString(reader.GetOrdinal("EstadoAprobacion")),
                ObservacionRevision = reader.IsDBNull(observacionOrdinal) ? null : reader.GetString(observacionOrdinal),
                FechaRevision = reader.IsDBNull(fechaRevisionOrdinal) ? null : reader.GetDateTime(fechaRevisionOrdinal),
                NombreRevisor = reader.IsDBNull(revisorOrdinal) ? null : reader.GetString(revisorOrdinal),
            };
        }
    }
}
