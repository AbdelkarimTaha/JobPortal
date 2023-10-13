namespace CleanArchitecture.Domain.DTOs
{
    public class VacancyGetDto : VacancyDto
    {
        public Guid Id { get; set; }
        public string? Location { get; set; }
        public string? VacancyType { get; set; }
        public string? Status { get; set; }
        public int? TotalApplications { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedByUser { get; set; }
        public string ModifiedByUser { get; set; }
        public bool? IsActive { get; set; }
    }
}
