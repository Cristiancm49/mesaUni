using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroApi.Seguridad.Domain.DTOs.Catalogo;
using MicroApi.Seguridad.Domain.DTOs.Common;
using MicroApi.Seguridad.Domain.Security;
using MicroApi.Seguridad.Domain.Interfaces.Services;
using MicroApi.Seguridad.Domain.Interfaces;
namespace MicroApi.Seguridad.Api.Controllers.Catalogo
{
    [ApiController]
    [Route("api/catalogo/sedes")]
    [Tags("Sede")]
    [Authorize(Roles = AppRoles.Todos)]
    public class SedeController : ControllerBase
    {
        private readonly ISedeService _service;

        public SedeController(ISedeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<SedeDto>>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(ApiResponseDto<IEnumerable<SedeDto>>.SuccessResponse(items, "Sedes obtenidas"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<SedeDto>>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseDto<SedeDto>.FailResponse("Sede no encontrada"));
            return Ok(ApiResponseDto<SedeDto>.SuccessResponse(item, "Sede obtenida"));
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<ActionResult<ApiResponseDto<SedeDto>>> Create([FromBody] SedeCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                ApiResponseDto<SedeDto>.SuccessResponse(created, "Sede creada"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<ActionResult<ApiResponseDto<SedeDto>>> Update(long id, [FromBody] SedeUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound(ApiResponseDto<SedeDto>.FailResponse("Sede no encontrada"));
            return Ok(ApiResponseDto<SedeDto>.SuccessResponse(updated, "Sede actualizada"));
        }
    }
}

