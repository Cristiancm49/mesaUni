using Chaira.MesaServicio.Domain.DTOs.Soporte;

namespace Chaira.MesaServicio.Domain.DTOs.Inventario
{
    public class ActivoHistorialDto
    {
        public ActivoDto? Activo { get; set; }
        public List<HojaDeVidaActivoDto> HojaDeVida { get; set; } = new();
        public List<CasoHistorialCompletoDto> Casos { get; set; } = new();
    }
}

