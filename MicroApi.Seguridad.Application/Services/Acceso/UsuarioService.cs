using MicroApi.Seguridad.Domain.DTOs.Acceso;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.Interfaces;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Models.Acceso;

namespace MicroApi.Seguridad.Application.Services.Acceso
{
    public class UsuarioService : GenericService<Usuario, UsuarioDto, UsuarioCreateDto, UsuarioUpdateDto>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository repository) : base(repository)
        {
            _usuarioRepository = repository;
        }

        protected override long GetEntityId(Usuario entity) => entity.Id;

        protected override UsuarioDto MapToDto(Usuario entity)
        {
            return new UsuarioDto
            {
                Id = entity.Id,
                NombreCompleto = entity.NombreCompleto,
                Email = entity.Email,
                Telefono = entity.Telefono,
                IdRol = entity.IdRol,
                NombreRol = entity.Rol?.NombreRol,
                FechaCreacion = entity.FechaCreacion
            };
        }

        protected override Usuario MapToEntity(UsuarioCreateDto createDto)
        {
            return new Usuario
            {
                NombreCompleto = createDto.NombreCompleto,
                Email = createDto.Email,
                Telefono = createDto.Telefono,
                IdRol = createDto.IdRol,
                IdUsuarioCreacion = createDto.IdUsuarioCreacion,
                FechaCreacion = DateTime.Now
            };
        }

        protected override void MapUpdateToEntity(UsuarioUpdateDto updateDto, Usuario entity)
        {
            if (!string.IsNullOrEmpty(updateDto.NombreCompleto))
                entity.NombreCompleto = updateDto.NombreCompleto;
            
            if (!string.IsNullOrEmpty(updateDto.Email))
                entity.Email = updateDto.Email;
            
            if (updateDto.Telefono != null)
                entity.Telefono = updateDto.Telefono;
            
            if (updateDto.IdRol.HasValue)
                entity.IdRol = updateDto.IdRol.Value;
        }

        // Sobrescribir GetAllAsync para incluir el Rol
        public override async Task<ApiResponseDto<IEnumerable<UsuarioDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _usuarioRepository.GetAllWithRolAsync();
                var dtos = entities.Select(MapToDto);
                return new ApiResponseDto<IEnumerable<UsuarioDto>>
                {
                    Success = true,
                    Message = "Registros obtenidos",
                    Data = dtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<IEnumerable<UsuarioDto>>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        // Sobrescribir GetByIdAsync para incluir el Rol
        public override async Task<ApiResponseDto<UsuarioDto>> GetByIdAsync(long id)
        {
            try
            {
                var entity = await _usuarioRepository.GetByIdWithRolAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<UsuarioDto>
                    {
                        Success = false,
                        Message = $"Usuario con ID {id} no encontrado",
                        Data = null
                    };
                }
                return new ApiResponseDto<UsuarioDto>
                {
                    Success = true,
                    Message = "Registro obtenido",
                    Data = MapToDto(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<UsuarioDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        // Sobrescribir CreateAsync para recargar con relaciones
        public override async Task<ApiResponseDto<UsuarioDto>> CreateAsync(UsuarioCreateDto createDto)
        {
            try
            {
                var entity = MapToEntity(createDto);
                var created = await _usuarioRepository.AddAsync(entity);
                var entityWithRol = await _usuarioRepository.GetByIdWithRolAsync(created.Id);
                return new ApiResponseDto<UsuarioDto>
                {
                    Success = true,
                    Message = "Usuario creado exitosamente",
                    Data = MapToDto(entityWithRol!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<UsuarioDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        // Sobrescribir UpdateAsync para recargar con relaciones
        public override async Task<ApiResponseDto<UsuarioDto>> UpdateAsync(long id, UsuarioUpdateDto updateDto)
        {
            try
            {
                var entity = await _usuarioRepository.GetByIdAsync(id);
                if (entity == null)
                {
                    return new ApiResponseDto<UsuarioDto>
                    {
                        Success = false,
                        Message = $"Usuario con ID {id} no encontrado",
                        Data = null
                    };
                }
                MapUpdateToEntity(updateDto, entity);
                await _usuarioRepository.UpdateAsync(entity);
                var entityWithRol = await _usuarioRepository.GetByIdWithRolAsync(id);
                return new ApiResponseDto<UsuarioDto>
                {
                    Success = true,
                    Message = "Usuario actualizado exitosamente",
                    Data = MapToDto(entityWithRol!)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<UsuarioDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        // Método adicional: buscar por email
        public async Task<ApiResponseDto<UsuarioDto>> GetByEmailAsync(string email)
        {
            try
            {
                var entity = await _usuarioRepository.GetByEmailAsync(email);
                if (entity == null)
                {
                    return new ApiResponseDto<UsuarioDto>
                    {
                        Success = false,
                        Message = $"Usuario con email '{email}' no encontrado",
                        Data = null
                    };
                }
                return new ApiResponseDto<UsuarioDto>
                {
                    Success = true,
                    Message = "Registro obtenido",
                    Data = MapToDto(entity)
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<UsuarioDto>
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}

