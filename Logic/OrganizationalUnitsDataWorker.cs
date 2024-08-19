using Database;
using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Logic
{
    public class OrganizationalUnitsDataWorker
    {
        private readonly MyDbContext _db;
        public OrganizationalUnitsDataWorker(MyDbContext db) { _db = db; }

        public async Task<IEnumerable<OrganizationalUnit>> GetOrganizationalUnitsAsync(DateTime? date)
        {
			if (date == null)
            {
				return await _db.OrganizationalUnits.Where(x => x.IsActive).ToListAsync();
			}
            else
            {
				return await _db.OrganizationalUnits
                    .Where(x => _db.OrganizationalUnitsChangeLogs.Any(cl => cl.ObjId == x.Id && cl.Date <= date) && !_db.OrganizationalUnitsChangeLogs.Any(cl => cl.ObjId == x.Id && cl.Date <= date && !cl.IsActive))
                    .ToListAsync();
			}
        }

        public async Task<OrganizationalUnit> GetOrganizationalUnitByIdAsync(Guid id)
        {
            OrganizationalUnit? foundOrganizationalUnit = await _db.OrganizationalUnits.FirstOrDefaultAsync(x => x.Id == id);
            
            if (foundOrganizationalUnit == null)
            {
                throw new Exception("Подразделение не найдено");
            }
            return foundOrganizationalUnit;
        }

        public async Task CreateOrganizationalUnitAsync(OrganizationalUnit organizationalUnit)
        {
            _db.OrganizationalUnits.Add(organizationalUnit);
            await _db.SaveChangesAsync();

            _db.OrganizationalUnitsChangeLogs.Add(new OrganizationalUnitsChangeLog
            {
                ObjId = organizationalUnit.Id,
                UnitName = organizationalUnit.UnitName,
                ParentId = organizationalUnit.ParentId,
                Date = GetChangeLogDateTime(),
				IsActive = true
            });
            await _db.SaveChangesAsync();
        }

        public async Task UpdateOrganizationalUnitAsync(OrganizationalUnit organizationalUnit)
        {
            OrganizationalUnit? updatedOrganizationalUnit = await _db.OrganizationalUnits.FirstOrDefaultAsync(x => x.Id == organizationalUnit.Id);
            if (updatedOrganizationalUnit == null)
            {
                throw new Exception("Подразделение не найдено");
            }

            updatedOrganizationalUnit.UnitName = organizationalUnit.UnitName;
            updatedOrganizationalUnit.ParentId = organizationalUnit.ParentId;
            await _db.SaveChangesAsync();

            _db.OrganizationalUnitsChangeLogs.Add(new OrganizationalUnitsChangeLog
            {
                ObjId = updatedOrganizationalUnit.Id,
                UnitName = updatedOrganizationalUnit.UnitName,
                ParentId = updatedOrganizationalUnit.ParentId,
                Date = GetChangeLogDateTime(),
				IsActive = updatedOrganizationalUnit.IsActive
			});
            await _db.SaveChangesAsync();
        }

        public async Task DeleteOrganizationalUnitAsync(Guid id)
        {
            OrganizationalUnit? remoteOrganizationalUnit = _db.OrganizationalUnits.FirstOrDefault(x => x.Id == id);
            if (remoteOrganizationalUnit == null)
            {
                throw new Exception("Подразделение не найдено");
            }
            remoteOrganizationalUnit.IsActive = false;

            _db.OrganizationalUnitsChangeLogs.Add(new OrganizationalUnitsChangeLog
            {
                ObjId = remoteOrganizationalUnit.Id,
                UnitName = remoteOrganizationalUnit.UnitName,
                ParentId = remoteOrganizationalUnit.ParentId,
                Date = GetChangeLogDateTime(),
				IsActive = false
            });
            await _db.SaveChangesAsync();
        }

        DateTime GetChangeLogDateTime()
        {
            var now = DateTime.Now;
            return new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        }
    }
}
