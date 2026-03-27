using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.Interfaces
{
    public interface IIntervencionTecnicaRepositoryExtended
    {
        // Métodos existentes (flujo viejo - mantener por compatibilidad)
        Task<dynamic> SpIntervencionTecnicaCrearAsync(IntervencionTecnicaCreateDto dto);
        Task<dynamic> SpIntervencionTecnicaActualizarAsync(IntervencionTecnicaUpdateDto dto);

        // Nuevos métodos (flujo dividido en 3 fases)
        Task<dynamic> SpIntervencionTecnicaCrearConDiagnosticoAsync(IntervencionDiagnosticoCreateDto dto);
        Task<dynamic> SpIntervencionTecnicaActualizarDiagnosticoAsync(IntervencionDiagnosticoUpdateDto dto);
        Task<dynamic> SpIntervencionTecnicaEjecutarAsync(IntervencionEjecutarDto dto);
        Task<dynamic> SpIntervencionTecnicaActualizarSolucionAsync(IntervencionSolucionUpdateDto dto);
    }
}



