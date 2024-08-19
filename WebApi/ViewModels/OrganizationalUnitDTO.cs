namespace WebApi.ViewModels
{
    public class OrganizationalUnitDTO
    {
        public Guid Id { get; set; }
        public string? UnitName { get; set; }
        public Guid? ParentId { get; set; }
    }
}
