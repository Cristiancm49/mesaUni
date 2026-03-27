using MicroApi.Seguridad.Domain.DTOs.Catalogo;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Application.Services.Catalogo
{
    public class EstadoGeneralService : GenericService<EstadoGeneral, EstadoGeneralDto, EstadoGeneralCreateDto, EstadoGeneralUpdateDto>, IEstadoGeneralService
    {
        public EstadoGeneralService(IGenericRepository<EstadoGeneral> repository) : base(repository)
        {
        }

        protected override EstadoGeneralDto MapToDto(EstadoGeneral entity)
        {
            return new EstadoGeneralDto
            {
                Id = entity.Id,
                NombreEstado = entity.NombreEstado,
                Descripcion = entity.Descripcion,
                Activo = entity.Activo,
                FechaCreacion = entity.FechaCreacion
            };
        }

        protected override EstadoGeneral MapToEntity(EstadoGeneralCreateDto dto)
        {
            return new EstadoGeneral
            {
                NombreEstado = dto.NombreEstado,
                Descripcion = dto.Descripcion,
                Activo = true,
                FechaCreacion = DateTime.UtcNow,
                IdUsuarioCreacion = dto.IdUsuarioCreacion
            };
        }

        protected override void MapUpdateToEntity(EstadoGeneralUpdateDto dto, EstadoGeneral entity)
        {
            if (!string.IsNullOrEmpty(dto.NombreEstado))
                entity.NombreEstado = dto.NombreEstado;

            if (dto.Descripcion != null)
                entity.Descripcion = dto.Descripcion;

            if (dto.Activo.HasValue)
                entity.Activo = dto.Activo.Value;
        }

        protected override long GetEntityId(EstadoGeneral entity) => entity.Id;
    }
}
