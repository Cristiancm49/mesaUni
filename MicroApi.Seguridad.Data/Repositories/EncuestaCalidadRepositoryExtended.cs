using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Data.Repositories
{
    public class EncuestaCalidadRepositoryExtended : IEncuestaCalidadRepositoryExtended
    {
        private readonly ApplicationDbContext _context;

        public EncuestaCalidadRepositoryExtended(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<EncuestaCalidadResponseDto> SpEncuestaCalidadCrearAsync(EncuestaCalidadCreateDto dto)
        {
            // Serializar respuestas a JSON
            string respuestasJson = JsonSerializer.Serialize(dto.Respuestas);

            var parameters = new[]
            {
                new SqlParameter("@IdCaso", dto.IdCaso),
                new SqlParameter("@Observaciones", (object?)dto.Observaciones ?? DBNull.Value),
                new SqlParameter("@IdUsuarioCreacion", dto.IdUsuarioCreacion),
                new SqlParameter("@RespuestasJSON", respuestasJson),
                new SqlParameter("@IdEncuestaNueva", SqlDbType.BigInt) { Direction = ParameterDirection.Output }
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spEncuestaCalidadCrear";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            
            EncuestaCalidadResponseDto? encuesta = null;

            // Result Set 1: Encuesta creada
            if (await reader.ReadAsync())
            {
                encuesta = new EncuestaCalidadResponseDto
                {
                    Id = reader.GetInt64(reader.GetOrdinal("Id")),
                    IdCaso = reader.GetInt64(reader.GetOrdinal("IdCaso")),
                    NumeroCaso = reader.GetString(reader.GetOrdinal("NumeroCaso")),
                    FechaEncuesta = reader.GetDateTime(reader.GetOrdinal("FechaEncuesta")),
                    Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? null : reader.GetString(reader.GetOrdinal("Observaciones")),
                    IdUsuarioCreacion = reader.GetInt64(reader.GetOrdinal("IdUsuarioCreacion")),
                    NombreUsuario = reader.GetString(reader.GetOrdinal("NombreUsuario")),
                    CantidadRespuestas = reader.GetInt32(reader.GetOrdinal("CantidadRespuestas")),
                    Respuestas = new List<DetalleRespuestaDto>()
                };
            }

            // Result Set 2: Respuestas de la encuesta
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    encuesta!.Respuestas!.Add(new DetalleRespuestaDto
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("Id")),
                        IdPregunta = reader.GetInt64(reader.GetOrdinal("IdPregunta")),
                        TextoPregunta = reader.GetString(reader.GetOrdinal("TextoPregunta")),
                        IdRespuesta = reader.GetInt64(reader.GetOrdinal("IdRespuesta")),
                        TextoRespuesta = reader.GetString(reader.GetOrdinal("TextoRespuesta")),
                        ValorNumerico = reader.IsDBNull(reader.GetOrdinal("ValorNumerico")) ? null : reader.GetInt32(reader.GetOrdinal("ValorNumerico")),
                        FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                    });
                }
            }

            return encuesta ?? throw new InvalidOperationException("No se pudo crear la encuesta de calidad");
        }
    }
}



