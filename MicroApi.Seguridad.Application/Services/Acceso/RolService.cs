using MicroApi.Seguridad.Domain.DTOs.Acceso;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Acceso;
using MicroApi.Seguridad.Domain.Models.Catalogo;

namespace MicroApi.Seguridad.Application.Services.Acceso
{
    public class RolService : GenericService<Rol, RolDto, RolCreateDto, RolUpdateDto>, IRolService
    {
        private readonly IGenericRepository<EstadoGeneral> _estadosGeneralesRepository;

        public RolService(
            IGenericRepository<Rol> repository,
            IGenericRepository<EstadoGeneral> estadosGeneralesRepository) : base(repository)
        {
            _estadosGeneralesRepository = estadosGeneralesRepository;
        }

        protected override long GetEntityId(Rol entity) => entity.Id;

        protected override RolDto MapToDto(Rol entity)
        {
            return new RolDto
            {
                Id = entity.Id,
                NombreRol = entity.NombreRol,
                Descripcion = entity.Descripcion,
                IdEstadoGeneral = entity.IdEstadoGeneral,
                NombreEstadoGeneral = entity.EstadoGeneral?.NombreEstado,
                FechaCreacion = entity.FechaCreacion
            };
        }

        protected override Rol MapToEntity(RolCreateDto createDto)
        {
            return new Rol
            {
                NombreRol = createDto.NombreRol,
                Descripcion = createDto.Descripcion,
                IdEstadoGeneral = createDto.IdEstadoGeneral,
                IdUsuarioCreacion = createDto.IdUsuarioCreacion,
                FechaCreacion = DateTime.Now
            };
        }

        protected override void MapUpdateToEntity(RolUpdateDto updateDto, Rol entity)
        {
            if (!string.IsNullOrWhiteSpace(updateDto.NombreRol))
            {
                entity.NombreRol = updateDto.NombreRol;
            }

            if (updateDto.Descripcion != null)
            {
                entity.Descripcion = updateDto.Descripcion;
            }

            if (updateDto.IdEstadoGeneral.HasValue)
            {
                entity.IdEstadoGeneral = updateDto.IdEstadoGeneral.Value;
            }
        }

        public async Task<ApiResponseDto<RolDto>> InactivarAsync(long id)
        {
            return await CambiarEstadoAsync(id, "Inactivo", "Rol inactivado");
        }

        public async Task<ApiResponseDto<RolDto>> ActivarAsync(long id)
        {
            return await CambiarEstadoAsync(id, "Activo", "Rol activado");
        }

        private async Task<ApiResponseDto<RolDto>> CambiarEstadoAsync(long id, string nombreEstado, string mensajeExito)
        {
            try
            {
                var entity = await _repository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<RolDto>
                    {
                        Success = false,
                        Message = $"Rol con ID {id} no encontrado",
                        Data = null
                    };
                }

                var estadoObjetivo = await _estadosGeneralesRepository.FirstOrDefaultAsync(
                    estado => estado.NombreEstado == nombreEstado);

                if (estadoObjetivo == null)
                {
                    return new ApiResponseDto<RolDto>
                    {
                        Success = false,
                        Message = $"No se encontro el estado general {nombreEstado}",
                        Data = null
                    };
                }

                entity.IdEstadoGeneral = estadoObjetivo.Id;
                var updated = await _repository.UpdateAsync(entity);

                return new ApiResponseDto<RolDto>
                {
                    Success = true,
                    Message = mensajeExito,
                    Data = MapToDto(updated)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<RolDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
