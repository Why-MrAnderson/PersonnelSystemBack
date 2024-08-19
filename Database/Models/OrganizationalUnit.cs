namespace Database.Models
{
    public class OrganizationalUnit
    {
        public Guid Id { get; set; }
        public string? UnitName { get; set; }
        public Guid? ParentId { get; set; }
        public OrganizationalUnit? Parent { get; set; }
        public List<OrganizationalUnit> Children { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();

        public bool IsActive { get; set; }
    }
}
