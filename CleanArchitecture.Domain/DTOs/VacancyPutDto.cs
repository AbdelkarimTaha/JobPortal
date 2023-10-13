namespace CleanArchitecture.Domain.DTOs
{
    public class VacancyPutDto : VacancyDto
    {
        public Guid Id { get; set; }
        public int? Location { get; set; }
        public int? VacancyType { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
