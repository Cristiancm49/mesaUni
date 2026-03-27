using MicroApi.Seguridad.Domain.DTOs.Catalogo;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Application.Services.Catalogo
{
    public class EstadoCasoService : GenericService<EstadoCaso, EstadoCasoDto, EstadoCasoCreateDto, EstadoCasoUpdateDto>, IEstadoCasoService
    {
        public EstadoCasoService(IGenericRepository<EstadoCaso> repository) : base(repository) { }

        protected override EstadoCasoDto MapToDto(EstadoCaso entity) => new()
        {
            Id = entity.Id,
            NombreEstadoCaso = entity.NombreEstadoCaso,
            DescripcionEstadoCaso = entity.DescripcionEstadoCaso,
            IdEstadoGeneral = entity.IdEstadoGeneral,
            NombreEstadoGeneral = entity.EstadoGeneral?.NombreEstado,
            FechaCreacion = entity.FechaCreacion
        };

        protected override EstadoCaso MapToEntity(EstadoCasoCreateDto dto) => new()
        {
            NombreEstadoCaso = dto.NombreEstadoCaso,
            DescripcionEstadoCaso = dto.DescripcionEstadoCaso,
            IdEstadoGeneral = dto.IdEstadoGeneral,
            FechaCreacion = DateTime.UtcNow,
            IdUsuarioCreacion = dto.IdUsuarioCreacion
        };

        protected override void MapUpdateToEntity(EstadoCasoUpdateDto dto, EstadoCaso entity)
        {
            if (!string.IsNullOrEmpty(dto.NombreEstadoCaso)) entity.NombreEstadoCaso = dto.NombreEstadoCaso;
            if (dto.DescripcionEstadoCaso != null) entity.DescripcionEstadoCaso = dto.DescripcionEstadoCaso;
            if (dto.IdEstadoGeneral.HasValue) entity.IdEstadoGeneral = dto.IdEstadoGeneral.Value;
        }

        protected override long GetEntityId(EstadoCaso entity) => entity.Id;
    }
}












