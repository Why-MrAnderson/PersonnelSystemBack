namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeesDataWorker _worker;
        public EmployeesController(EmployeesDataWorker worker) { _worker = worker; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetEmployees(DateTime? start, DateTime? end, Guid? organizationalUnit)
        {
            IEnumerable<Employee> result = await _worker.GetEmployeesAsync(start, end, organizationalUnit);
            return Ok(result.Select(EmployeeToDTO));
        } 

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDTO>> GetEmployeeById([FromRoute]Guid id)
        {
            try
            {
                return Ok(EmployeeToDTO(await _worker.GetEmployeeByIdAsync(id)));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDTO>> CreateEmployee([FromBody]EmployeeDTO employeeDTO)
        {
            Employee createdEmployee = new Employee
            {
                Name = employeeDTO.Name,
                Surname = employeeDTO.Surname,
                Patronymic = employeeDTO.Patronymic,
                OrganizationalUnitId = employeeDTO.OrganizationalUnitId,
                IsActive = true
            };

            await _worker.CreateEmployeeAsync(createdEmployee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, EmployeeToDTO(createdEmployee));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDTO employeeDTO)
        {
            Employee employee = new Employee
            {
                Id = employeeDTO.Id,
                Name = employeeDTO.Name,
                Surname = employeeDTO.Surname,
                Patronymic = employeeDTO.Patronymic,
                OrganizationalUnitId = employeeDTO.OrganizationalUnitId
            };

            try
            {
                await _worker.UpdateEmployeeAsync(employee);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                await _worker.DeleteEmployeeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        private static EmployeeDTO EmployeeToDTO(Employee employee)
        {
            return new EmployeeDTO
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Patronymic = employee.Patronymic,
                OrganizationalUnitId = employee.OrganizationalUnitId
            };
        }
    }
}
