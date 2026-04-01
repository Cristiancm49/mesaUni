using Chaira.MesaServicio.Domain.DTOs.Common;

namespace Chaira.MesaServicio.Domain.Interfaces
{
    public interface IGenericService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : class
        where TDto : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        Task<ApiResponseDto<IEnumerable<TDto>>> GetAllAsync();
        Task<ApiResponseDto<TDto>> GetByIdAsync(long id);
        Task<ApiResponseDto<TDto>> CreateAsync(TCreateDto createDto);
        Task<ApiResponseDto<TDto>> UpdateAsync(long id, TUpdateDto updateDto);
        Task<ApiResponseDto<int>> CountAsync();
    }
}

