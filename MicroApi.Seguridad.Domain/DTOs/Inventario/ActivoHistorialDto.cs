using MicroApi.Seguridad.Domain.DTOs.Soporte;

namespace MicroApi.Seguridad.Domain.DTOs.Inventario
{
    public class ActivoHistorialDto
    {
        public ActivoDto? Activo { get; set; }
        public List<HojaDeVidaActivoDto> HojaDeVida { get; set; } = new();
        public List<CasoHistorialCompletoDto> Casos { get; set; } = new();
    }
}
