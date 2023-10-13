namespace CleanArchitecture.Domain.Models
{
    public class BaseEntity
    {
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}
