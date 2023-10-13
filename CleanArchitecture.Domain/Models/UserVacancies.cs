using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Models
{
    public class UserVacancies : BaseEntity
    {
        [Key]
        public Guid VacancyId { get; set; }
        [Key]
        public Guid UserId { get; set; }
        public virtual Vacancy Vacancy { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
