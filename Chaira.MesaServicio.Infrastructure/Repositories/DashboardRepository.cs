using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.DTOs.Soporte;

namespace Chaira.MesaServicio.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardEstadisticasDto> SpDashboardEstadisticasCasosAsync(DashboardEstadisticasRequestDto request)
        {
            var parameters = new[]
            {
                new SqlParameter("@FechaDesde", (object?)request.FechaDesde ?? DBNull.Value),
                new SqlParameter("@FechaHasta", (object?)request.FechaHasta ?? DBNull.Value),
                new SqlParameter("@IdAreaTecnica", (object?)request.IdAreaTecnica ?? DBNull.Value),
                new SqlParameter("@IdTecnico", (object?)request.IdTecnico ?? DBNull.Value)
            };

            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "soporte.spDashboardEstadisticasCasos";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            await _context.Database.OpenConnectionAsync();

            using var reader = await command.ExecuteReaderAsync();
            
            var dashboard = new DashboardEstadisticasDto();

            // Result Set 1: Resumen general
            if (await reader.ReadAsync())
            {
                dashboard.ResumenGeneral = new ResumenGeneralDto
                {
                    TotalCasos = reader.GetInt32(reader.GetOrdinal("TotalCasos")),
                    CasosAbiertos = reader.GetInt32(reader.GetOrdinal("CasosAbiertos")),
                    CasosResueltos = reader.GetInt32(reader.GetOrdinal("CasosResueltos")),
                    CasosCerrados = reader.GetInt32(reader.GetOrdinal("CasosCerrados")),
                    CasosSinAsignar = reader.GetInt32(reader.GetOrdinal("CasosSinAsignar")),
                    TiempoPromedioResolucionHoras = reader.IsDBNull(reader.GetOrdinal("TiempoPromedioResolucionHoras")) 
                        ? null 
                        : reader.GetDouble(reader.GetOrdinal("TiempoPromedioResolucionHoras")),
                    TiempoPromedioCierreHoras = reader.IsDBNull(reader.GetOrdinal("TiempoPromedioCierreHoras")) 
                        ? null 
                        : reader.GetDouble(reader.GetOrdinal("TiempoPromedioCierreHoras"))
                };
            }

            // Result Set 2: Casos por estado
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    dashboard.CasosPorEstado.Add(new CasosPorEstadoDto
                    {
                        IdEstadoCaso = reader.GetInt64(reader.GetOrdinal("IdEstadoCaso")),
                        NombreEstadoCaso = reader.GetString(reader.GetOrdinal("NombreEstadoCaso")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                        Porcentaje = reader.GetDecimal(reader.GetOrdinal("Porcentaje"))
                    });
                }
            }

            // Result Set 3: Casos por prioridad
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    dashboard.CasosPorPrioridad.Add(new CasosPorPrioridadDto
                    {
                        IdPrioridad = reader.GetInt64(reader.GetOrdinal("IdPrioridad")),
                        NombrePrioridad = reader.GetString(reader.GetOrdinal("NombrePrioridad")),
                        Color = reader.GetString(reader.GetOrdinal("Color")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                        CasosAbiertos = reader.GetInt32(reader.GetOrdinal("CasosAbiertos")),
                        CasosCerrados = reader.GetInt32(reader.GetOrdinal("CasosCerrados"))
                    });
                }
            }

            // Result Set 4: Casos por tÃ©cnico
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    dashboard.CasosPorTecnico.Add(new CasosPorTecnicoDto
                    {
                        IdUsuario = reader.GetInt64(reader.GetOrdinal("IdUsuario")),
                        NombreCompleto = reader.GetString(reader.GetOrdinal("NombreCompleto")),
                        CasosAsignados = reader.GetInt32(reader.GetOrdinal("CasosAsignados")),
                        CasosResueltos = reader.GetInt32(reader.GetOrdinal("CasosResueltos")),
                        CasosCerrados = reader.GetInt32(reader.GetOrdinal("CasosCerrados")),
                        CasosEnProceso = reader.GetInt32(reader.GetOrdinal("CasosEnProceso")),
                        TiempoPromedioResolucionHoras = reader.IsDBNull(reader.GetOrdinal("TiempoPromedioResolucionHoras")) 
                            ? null 
                            : reader.GetDouble(reader.GetOrdinal("TiempoPromedioResolucionHoras"))
                    });
                }
            }

            // Result Set 5: Casos por Ã¡rea tÃ©cnica
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    dashboard.CasosPorArea.Add(new CasosPorAreaDto
                    {
                        IdAreaTecnica = reader.GetInt64(reader.GetOrdinal("IdAreaTecnica")),
                        NombreAreaTecnica = reader.GetString(reader.GetOrdinal("NombreAreaTecnica")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                        CasosAbiertos = reader.GetInt32(reader.GetOrdinal("CasosAbiertos")),
                        CasosCerrados = reader.GetInt32(reader.GetOrdinal("CasosCerrados")),
                        TiempoPromedioResolucionHoras = reader.IsDBNull(reader.GetOrdinal("TiempoPromedioResolucionHoras")) 
                            ? null 
                            : reader.GetDouble(reader.GetOrdinal("TiempoPromedioResolucionHoras"))
                    });
                }
            }

            // Result Set 6: Casos por tipo
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    dashboard.CasosPorTipo.Add(new CasosPorTipoDto
                    {
                        IdTipoCaso = reader.GetInt64(reader.GetOrdinal("IdTipoCaso")),
                        NombreTipoCaso = reader.GetString(reader.GetOrdinal("NombreTipoCaso")),
                        Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                        Porcentaje = reader.GetDecimal(reader.GetOrdinal("Porcentaje"))
                    });
                }
            }

            // Result Set 7: Tendencia diaria
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    dashboard.TendenciaDiaria.Add(new TendenciaDiariaDto
                    {
                        Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                        CasosCreados = reader.GetInt32(reader.GetOrdinal("CasosCreados")),
                        CasosResueltos = reader.GetInt32(reader.GetOrdinal("CasosResueltos")),
                        CasosCerrados = reader.GetInt32(reader.GetOrdinal("CasosCerrados"))
                    });
                }
            }

            // Result Set 8: Estado SLA
            if (await reader.NextResultAsync())
            {
                if (await reader.ReadAsync())
                {
                    dashboard.EstadoSLA = new EstadoSLADto
                    {
                        CasosEnTiempo = reader.GetInt32(reader.GetOrdinal("CasosEnTiempo")),
                        CasosProximosVencer = reader.GetInt32(reader.GetOrdinal("CasosProximosVencer")),
                        CasosVencidos = reader.GetInt32(reader.GetOrdinal("CasosVencidos")),
                        CasosCerradosEnSLA = reader.GetInt32(reader.GetOrdinal("CasosCerradosEnSLA")),
                        CasosCerradosFueraSLA = reader.GetInt32(reader.GetOrdinal("CasosCerradosFueraSLA"))
                    };
                }
            }

            return dashboard;
        }
    }
}




