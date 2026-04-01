using Microsoft.Extensions.DependencyInjection;
using Chaira.MesaServicio.Application.Common.Interfaces;
using Chaira.MesaServicio.Infrastructure.Persistence.Dapper;
using Chaira.MesaServicio.Infrastructure.Repositories;
using Chaira.MesaServicio.Domain.Interfaces;

namespace Chaira.MesaServicio.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IAreaTecnicaRepository, AreaTecnicaRepository>();
        services.AddScoped<ICanalIngresoRepository, CanalIngresoRepository>();
        services.AddScoped<IPrioridadRepository, PrioridadRepository>();
        services.AddScoped<ITipoCasoRepository, TipoCasoRepository>();
        services.AddScoped<ITipoTrabajoRepository, TipoTrabajoRepository>();
        services.AddScoped<ISedeRepository, SedeRepository>();

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        services.AddScoped<ICategoriaActivoRepository, CategoriaActivoRepository>();
        services.AddScoped<ITipoConsumibleRepository, TipoConsumibleRepository>();

        services.AddScoped<IUbicacionRepository, UbicacionRepository>();
        services.AddScoped<IInventarioRepository, InventarioRepository>();
        services.AddScoped<IComponenteRepository, ComponenteRepository>();
        services.AddScoped<IConsumibleRepository, ConsumibleRepository>();
        services.AddScoped<IActivoRepository, ActivoRepository>();
        services.AddScoped<IHojaDeVidaActivoRepository, HojaDeVidaActivoRepository>();

        services.AddScoped<ICasoRepository, CasoRepository>();
        services.AddScoped<ITrazabilidadCasoRepository, TrazabilidadCasoRepository>();
        services.AddScoped<IIntervencionTecnicaRepository, IntervencionTecnicaRepository>();
        services.AddScoped<IDetalleCambioComponentesRepository, DetalleCambioComponentesRepository>();
        services.AddScoped<IDetalleConsumibleRepository, DetalleConsumibleRepository>();
        services.AddScoped<IRevisionAdmiRepository, RevisionAdmiRepository>();
        services.AddScoped<IEncuestaCalidadRepository, EncuestaCalidadRepository>();
        services.AddScoped<IDetalleEncuestaRepository, DetalleEncuestaRepository>();

        services.AddScoped<IIntervencionTecnicaRepositoryExtended, IntervencionTecnicaRepositoryExtended>();
        services.AddScoped<IRevisionAdmiRepositoryExtended, RevisionAdmiRepositoryExtended>();
        services.AddScoped<IEncuestaCalidadRepositoryExtended, EncuestaCalidadRepositoryExtended>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        services.AddScoped<IReporteConfiguracionRepository, ReporteConfiguracionRepository>();
        services.AddScoped<IReporteSoporteRepository, ReporteSoporteRepository>();
        services.AddScoped<IEvidenciaRepository, EvidenciaRepository>();

        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IDapperQueryExecutor, DapperQueryExecutor>();

        return services;
    }
}

