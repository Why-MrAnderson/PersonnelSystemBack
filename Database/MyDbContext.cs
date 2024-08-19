using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class MyDbContext: DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<EmployeesChangeLog> EmployeesChangeLogs { get; set; } = null!;
        public DbSet<OrganizationalUnit> OrganizationalUnits { get; set; } = null!;
        public DbSet<OrganizationalUnitsChangeLog> OrganizationalUnitsChangeLogs { get; set; } = null!;
    }
}
