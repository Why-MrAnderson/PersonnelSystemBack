namespace Database.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname{ get; set; }
        public string? Patronymic { get; set; }
        public Guid? OrganizationalUnitId { get; set; }
        public OrganizationalUnit? OrganizationalUnit { get; set; }

        public bool IsActive { get; set; }
    }
}
