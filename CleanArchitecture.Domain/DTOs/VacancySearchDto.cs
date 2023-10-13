namespace CleanArchitecture.Domain.DTOs
{
    public class VacancySearchDto
    {
        public Guid? Id { get; set; }
        public string Title { get; set; } = null;
        public string Field { get; set; } = null;
        public string Requirements { get; set; } = null;
        public DateTime? ExpiryDate { get; set; }
        public int? Location { get; set; }
        public int? VacancyType { get; set; }
        public int? Status { get; set; }
    }
}
