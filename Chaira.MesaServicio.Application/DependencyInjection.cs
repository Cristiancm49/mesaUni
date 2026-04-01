using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Chaira.MesaServicio.Application.Common.Behavior;
using Chaira.MesaServicio.Application.Services.Acceso;
using Chaira.MesaServicio.Application.Services.Auth;
using Chaira.MesaServicio.Application.Services.Catalogo;
using Chaira.MesaServicio.Application.Services.Infrastructure;
using Chaira.MesaServicio.Application.Services.Inventario;
using Chaira.MesaServicio.Application.Services.Soporte;
using Chaira.MesaServicio.Domain.Interfaces.Services;

namespace Chaira.MesaServicio.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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

