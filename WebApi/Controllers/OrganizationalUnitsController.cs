namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrganizationalUnitsController : ControllerBase
    {
        private readonly OrganizationalUnitsDataWorker _worker;
        public OrganizationalUnitsController(OrganizationalUnitsDataWorker worker) { _worker = worker; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationalUnitDTO>>> GetOrganizationalUnits([FromQuery]DateTime? date)
        {
            IEnumerable<OrganizationalUnit> result = await _worker.GetOrganizationalUnitsAsync(date);
            return Ok(result.Select(UnitToDTO));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationalUnitDTO>> GetOrganizationalUnitById(Guid id)
        {
            try
            {
                return Ok(UnitToDTO(await _worker.GetOrganizationalUnitByIdAsync(id)));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrganizationalUnitDTO>> CreateOrganizationalUnit([FromBody] OrganizationalUnitDTO organizationalUnitDTO)
        {
            OrganizationalUnit createdOrganizationalUnit = new OrganizationalUnit
            {
                UnitName = organizationalUnitDTO.UnitName,
                ParentId = organizationalUnitDTO.ParentId != Guid.Empty ? organizationalUnitDTO.ParentId : null,
                IsActive = true
			};

            await _worker.CreateOrganizationalUnitAsync(createdOrganizationalUnit);
            return CreatedAtAction(nameof(GetOrganizationalUnitById), new { id = createdOrganizationalUnit.Id }, UnitToDTO(createdOrganizationalUnit));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrganizationalUnit([FromBody] OrganizationalUnitDTO organizationalUnitDTO)
        {
            OrganizationalUnit organizationalUnit = new OrganizationalUnit
            {
                Id = organizationalUnitDTO.Id,
                UnitName = organizationalUnitDTO.UnitName,
                ParentId = organizationalUnitDTO.ParentId
            };

            try
            {
                await _worker.UpdateOrganizationalUnitAsync(organizationalUnit);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrganizationalUnit(Guid id)
        {
            try
            {
                await _worker.DeleteOrganizationalUnitAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        private static OrganizationalUnitDTO UnitToDTO(OrganizationalUnit organizationalUnit)
        {
            return new OrganizationalUnitDTO
            {
                Id = organizationalUnit.Id,
                UnitName = organizationalUnit.UnitName,
                ParentId = organizationalUnit.ParentId
            };
        }
    }
}
