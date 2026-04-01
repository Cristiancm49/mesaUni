using Chaira.MesaServicio.Application.Services.Acceso;
using Chaira.MesaServicio.Application.Services.Auth;
using Chaira.MesaServicio.Application.Services.Catalogo;
using Chaira.MesaServicio.Application.Services.Infrastructure;
using Chaira.MesaServicio.Application.Services.Inventario;
using Chaira.MesaServicio.Application.Services.Soporte;
using Chaira.MesaServicio.Infrastructure.Repositories;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;

namespace Chaira.MesaServicio.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
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
            
            // Repositorios extendidos para Stored Procedures
            services.AddScoped<IIntervencionTecnicaRepositoryExtended, IntervencionTecnicaRepositoryExtended>();
            services.AddScoped<IRevisionAdmiRepositoryExtended, RevisionAdmiRepositoryExtended>();
            services.AddScoped<IEncuestaCalidadRepositoryExtended, EncuestaCalidadRepositoryExtended>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IReporteConfiguracionRepository, ReporteConfiguracionRepository>();
            services.AddScoped<IReporteSoporteRepository, ReporteSoporteRepository>();
            services.AddScoped<IEvidenciaRepository, EvidenciaRepository>();
            
            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IEstadoGeneralService, EstadoGeneralService>();
            services.AddScoped<IAreaTecnicaService, AreaTecnicaService>();
            services.AddScoped<ICanalIngresoService, CanalIngresoService>();
            services.AddScoped<ITipoCasoService, TipoCasoService>();
            services.AddScoped<IEstadoCasoService, EstadoCasoService>();
            services.AddScoped<IEstadoActivoService, EstadoActivoService>();
            services.AddScoped<IEstadoConsumibleService, EstadoConsumibleService>();
            services.AddScoped<IEstadoIntervencionTecnicaService, EstadoIntervencionTecnicaService>();
            services.AddScoped<IPrioridadService, PrioridadService>();
            services.AddScoped<ITipoTrabajoService, TipoTrabajoService>();
            services.AddScoped<ISedeService, SedeService>();
            
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IAuthService, AuthService>();
            
            services.AddScoped<IUbicacionService, UbicacionService>();
            services.AddScoped<ICategoriaActivoService, CategoriaActivoService>();
            services.AddScoped<ITipoConsumibleService, TipoConsumibleService>();
            services.AddScoped<IInventarioService, InventarioService>();
            services.AddScoped<IComponenteService, ComponenteService>();
            services.AddScoped<IConsumibleService, ConsumibleService>();
            services.AddScoped<IActivoService, ActivoService>();
            services.AddScoped<IHojaDeVidaActivoService, HojaDeVidaActivoService>();
            
            services.AddScoped<ICasoService, CasoService>();
            services.AddScoped<ITrazabilidadCasoService, TrazabilidadCasoService>();
            services.AddScoped<IIntervencionTecnicaService, IntervencionTecnicaService>();
            services.AddScoped<IDetalleCambioComponentesService, DetalleCambioComponentesService>();
            services.AddScoped<IDetalleConsumibleService, DetalleConsumibleService>();
            services.AddScoped<IRevisionAdmiService, RevisionAdmiService>();
            services.AddScoped<IEncuestaCalidadService, EncuestaCalidadService>();
            services.AddScoped<IDetalleEncuestaService, DetalleEncuestaService>();
            services.AddScoped<IEvidenciaService, EvidenciaService>();
            
            return services;
        }
    }
}

