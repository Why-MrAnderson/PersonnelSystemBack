namespace Database.Models
{
    public class EmployeesChangeLog
    {
        public Guid Id { get; set; }
        public Guid ObjId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public Guid? OrganizationalUnitId { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
