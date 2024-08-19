using Database;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Logic
{
    public class EmployeesDataWorker
    {
        private readonly MyDbContext _db;
        public EmployeesDataWorker(MyDbContext db) { _db = db; }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(DateTime? start, DateTime? end, Guid? organizationalUnitId)
        {
			if (start != null && end != null && organizationalUnitId != null)
            {
				return await _db.Employees
                    .Where
                    (
                        e => 
                        _db.EmployeesChangeLogs.Any(cl => cl.ObjId == e.Id && cl.OrganizationalUnitId == organizationalUnitId && cl.IsActive && cl.Date <= end) 
                        && 
                        (   
                            _db.EmployeesChangeLogs.FirstOrDefault
                            (
                                cl => 
                                cl.ObjId == e.Id 
                                    && (cl.OrganizationalUnitId != organizationalUnitId || !cl.IsActive) 
                                    && (cl.Date < start && cl.Date > _db.EmployeesChangeLogs.OrderBy(x => x.Date).LastOrDefault(cl => cl.ObjId == e.Id && cl.OrganizationalUnitId == organizationalUnitId && cl.IsActive && cl.Date <= end).Date)
                            ) == null
                        )
                    )
                    .ToListAsync();
			}
			else
            {
				return await _db.Employees.Where(e => e.IsActive).ToListAsync();
			}
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            Employee? foundEmployee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (foundEmployee == null)
            {
                throw new Exception("Сотрудник не найден");
            }
            return foundEmployee;
        }

        public async Task CreateEmployeeAsync(Employee employee)
        {
            _db.Employees.Add(employee);
            await _db.SaveChangesAsync();

            _db.EmployeesChangeLogs.Add(new EmployeesChangeLog
            {
                ObjId=employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Patronymic = employee.Patronymic,
                OrganizationalUnitId = employee.OrganizationalUnitId,
                Date = DateTime.Now,
                IsActive = employee.IsActive
			});
            await _db.SaveChangesAsync();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            Employee? updatedEmployee = await _db.Employees.FirstOrDefaultAsync(x => x.Id == employee.Id);
            if (updatedEmployee == null)
            {
                throw new Exception("Сотрудник не найден");
            }

            updatedEmployee.Name = employee.Name;
            updatedEmployee.Surname = employee.Surname;
            updatedEmployee.Patronymic = employee.Patronymic;
            updatedEmployee.OrganizationalUnitId = employee.OrganizationalUnitId;
            await _db.SaveChangesAsync();

            _db.EmployeesChangeLogs.Add(new EmployeesChangeLog
            {
                ObjId = updatedEmployee.Id,
                Name = updatedEmployee.Name,
                Surname = updatedEmployee.Surname,
                Patronymic = updatedEmployee.Patronymic,
                OrganizationalUnitId = updatedEmployee.OrganizationalUnitId,
                Date = DateTime.Now,
                IsActive = updatedEmployee.IsActive
			});
            await _db.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            Employee? remoteEmployee = _db.Employees.FirstOrDefault(x => x.Id == id);
            if (remoteEmployee == null)
            {
                throw new Exception("Сотрудник не найден");
            }
            remoteEmployee.IsActive = false;

            _db.EmployeesChangeLogs.Add(new EmployeesChangeLog
            {
                ObjId = remoteEmployee.Id,
                Name = remoteEmployee.Name,
                Surname = remoteEmployee.Surname,
                Patronymic = remoteEmployee.Patronymic,
                OrganizationalUnitId = remoteEmployee.OrganizationalUnitId,
                Date = DateTime.Now,
                IsActive = false
            });
            await _db.SaveChangesAsync();
        }
    }
}
