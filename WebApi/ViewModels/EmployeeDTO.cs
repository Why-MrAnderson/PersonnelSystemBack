namespace WebApi.ViewModels
{
    public class EmployeeDTO
    {
        public EmployeeDTO() { }
		public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public Guid? OrganizationalUnitId { get; set; }
    }
}
