using Chaira.MesaServicio.Domain.DTOs.Soporte;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IIntervencionTecnicaRepositoryExtended
    {
        // MÃ©todos existentes (flujo viejo - mantener por compatibilidad)
        Task<dynamic> SpIntervencionTecnicaCrearAsync(IntervencionTecnicaCreateDto dto);
        Task<dynamic> SpIntervencionTecnicaActualizarAsync(IntervencionTecnicaUpdateDto dto);

        // Nuevos mÃ©todos (flujo dividido en 3 fases)
        Task<dynamic> SpIntervencionTecnicaCrearConDiagnosticoAsync(IntervencionDiagnosticoCreateDto dto);
        Task<dynamic> SpIntervencionTecnicaActualizarDiagnosticoAsync(IntervencionDiagnosticoUpdateDto dto);
        Task<dynamic> SpIntervencionTecnicaEjecutarAsync(IntervencionEjecutarDto dto);
        Task<dynamic> SpIntervencionTecnicaActualizarSolucionAsync(IntervencionSolucionUpdateDto dto);
    }
}




