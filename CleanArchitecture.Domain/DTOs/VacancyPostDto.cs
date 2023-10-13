namespace CleanArchitecture.Domain.DTOs
{
    public class VacancyPostDto : VacancyDto
    {
        public int? Location { get; set; }
        public int? VacancyType { get; set; }
        public string CreatedBy { get; set; }
    }
}
