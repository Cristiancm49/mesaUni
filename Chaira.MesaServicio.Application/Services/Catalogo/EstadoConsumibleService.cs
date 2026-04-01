using Chaira.MesaServicio.Domain.DTOs.Catalogo;
using Chaira.MesaServicio.Domain.Interfaces;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Models.Catalogo;

namespace Chaira.MesaServicio.Application.Services.Catalogo
{
    public class EstadoConsumibleService : GenericService<EstadoConsumible, EstadoConsumibleDto, EstadoConsumibleCreateDto, EstadoConsumibleUpdateDto>, IEstadoConsumibleService
    {
        public EstadoConsumibleService(IGenericRepository<EstadoConsumible> repository) : base(repository) { }

        protected override EstadoConsumibleDto MapToDto(EstadoConsumible entity) => new()
        {
            Id = entity.Id,
            NombreEstado = entity.NombreEstado,
            Descripcion = entity.Descripcion,
            IdEstadoGeneral = entity.IdEstadoGeneral,
            NombreEstadoGeneral = entity.EstadoGeneral?.NombreEstado,
            FechaCreacion = entity.FechaCreacion
        };

        protected override EstadoConsumible MapToEntity(EstadoConsumibleCreateDto dto) => new()
        {
            NombreEstado = dto.NombreEstado,
            Descripcion = dto.Descripcion,
            IdEstadoGeneral = dto.IdEstadoGeneral,
            FechaCreacion = DateTime.UtcNow,
            IdUsuarioCreacion = dto.IdUsuarioCreacion
        };

        protected override void MapUpdateToEntity(EstadoConsumibleUpdateDto dto, EstadoConsumible entity)
        {
            if (!string.IsNullOrEmpty(dto.NombreEstado)) entity.NombreEstado = dto.NombreEstado;
            if (dto.Descripcion != null) entity.Descripcion = dto.Descripcion;
            if (dto.IdEstadoGeneral.HasValue) entity.IdEstadoGeneral = dto.IdEstadoGeneral.Value;
        }

        protected override long GetEntityId(EstadoConsumible entity) => entity.Id;
    }
}















