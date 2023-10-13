using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Domain.Models
{
    public class UserVacancies : BaseEntity
    {
        [Key]
        public Guid VacancyId { get; private set; }
        [Key]
        public string UserId { get; private set; }
        public virtual Vacancy Vacancy { get; set; }
        public virtual ApplicationUser User { get; set; }
        private UserVacancies()
        {

        }
        private UserVacancies(Guid vacancyId, string? currentUser, string userId)
        {
            VacancyId = vacancyId;
            UserId = userId;
            CreatedDate = DateTime.Now;
            CreatedBy = currentUser;
    }
        public static UserVacancies Create(Guid vacancyId, string? currentUser, string userId)
        {
            return new UserVacancies(vacancyId, currentUser, userId);
        }
    }
}
