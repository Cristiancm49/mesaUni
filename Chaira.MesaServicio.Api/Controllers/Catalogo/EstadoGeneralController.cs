using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Chaira.MesaServicio.Domain.DTOs.Catalogo;
using Chaira.MesaServicio.Domain.DTOs.Common;
using Chaira.MesaServicio.Domain.Security;
using Chaira.MesaServicio.Domain.Interfaces.Services;
using Chaira.MesaServicio.Domain.Interfaces;
namespace Chaira.MesaServicio.Api.Controllers.Catalogo
{
    [ApiController]
    [Route("api/catalogo/estados-generales")]
    [Tags("EstadoGeneral")]
    [Produces("application/json")]
    [Authorize(Roles = AppRoles.Todos)]
    public class EstadoGeneralController : ControllerBase
    {
        private readonly IEstadoGeneralService _service;

        public EstadoGeneralController(IEstadoGeneralService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> ObtenerPorId(long id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> ContarTotal()
        {
            var result = await _service.CountAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<IActionResult> Crear([FromBody] EstadoGeneralCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            
            if (!result.Success)
                return BadRequest(result);
            
            return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Data?.Id }, result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = AppRoles.Administrador)]
        public async Task<IActionResult> Actualizar(long id, [FromBody] EstadoGeneralUpdateDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}


