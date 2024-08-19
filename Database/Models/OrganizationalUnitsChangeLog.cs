namespace Database.Models
{
    public class OrganizationalUnitsChangeLog
    {
        public Guid Id { get; set; }
        public Guid ObjId { get; set; }
        public string? UnitName { get; set; }
        public Guid? ParentId { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}
