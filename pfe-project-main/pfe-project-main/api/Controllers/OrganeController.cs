using Microsoft.AspNetCore.Mvc;
using PFE_PROJECT.Models;
using PFE_PROJECT.Services;
using PFE_PROJECT.Helpers; // pour RoleHelper
using Microsoft.AspNetCore.Authorization;

namespace PFE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganeController : ControllerBase
    {
        private readonly IOrganeService _service;

        public OrganeController(IOrganeService service)
        {
            _service = service;
        }

        // GET: api/Organe
        [HttpGet]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<IEnumerable<OrganeDTO>>> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = true)
        {
            return Ok(await _service.GetAllAsync(search, sortBy, ascending));
        }

        // GET: api/Organe/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<OrganeDTO>> GetById(int id)
        {
            var o = await _service.GetByIdAsync(id);
            return o == null ? NotFound() : Ok(o);
        }

        // POST: api/Organe
        [HttpPost]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<ActionResult<OrganeDTO>> Create(CreateOrganeDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.id_organe }, created);
        }

        // PUT: api/Organe/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> Update(int id, UpdateOrganeDTO dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        // DELETE: api/Organe/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin Métier,Admin IT")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _service.CanDeleteAsync(id))
                return BadRequest("Cet organe est utilisé par un équipement.");

            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
