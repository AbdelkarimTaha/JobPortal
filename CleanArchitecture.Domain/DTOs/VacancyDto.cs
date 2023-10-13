using CleanArchitecture.Domain.Enums;

namespace CleanArchitecture.Domain.DTOs
{
    public class VacancyDto
    {
        public string Title { get; set; }
        public string Field { get; set; }
        public string Requirements { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? MaxApplications { get; set; }
    }
}
