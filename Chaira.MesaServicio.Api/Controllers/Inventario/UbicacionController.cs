using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.DTOs.Inventario;
using Chaira.MesaServicio.Domain.Security;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Interfaces;
namespace Chaira.MesaServicio.Api.Controllers.Inventario
{
    [ApiController]
    [Route("api/inventario/ubicacion")]
    [Tags("Ubicacion")]
    [Authorize(Roles = AppRoles.TecnicoAdministrativoAdministrador)]
    public class UbicacionController : ControllerBase
    {
        private readonly IUbicacionService _service;

        public UbicacionController(IUbicacionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<IEnumerable<UbicacionDto>>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(ApiResponseDto<IEnumerable<UbicacionDto>>.SuccessResponse(items, "Ubicaciones obtenidas"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<UbicacionDto>>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(ApiResponseDto<UbicacionDto>.FailResponse("UbicaciÃ³n no encontrada"));
            return Ok(ApiResponseDto<UbicacionDto>.SuccessResponse(item, "UbicaciÃ³n obtenida"));
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<UbicacionDto>>> Create([FromBody] UbicacionCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id },
                ApiResponseDto<UbicacionDto>.SuccessResponse(created, "UbicaciÃ³n creada"));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<UbicacionDto>>> Update(long id, [FromBody] UbicacionUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound(ApiResponseDto<UbicacionDto>.FailResponse("UbicaciÃ³n no encontrada"));
            return Ok(ApiResponseDto<UbicacionDto>.SuccessResponse(updated, "UbicaciÃ³n actualizada"));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = AppRoles.AdministrativoAdministrador)]
        public async Task<ActionResult<ApiResponseDto<bool>>> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(ApiResponseDto<bool>.FailResponse("UbicaciÃ³n no encontrada"));
            return Ok(ApiResponseDto<bool>.SuccessResponse(true, "UbicaciÃ³n eliminada"));
        }
    }
}


