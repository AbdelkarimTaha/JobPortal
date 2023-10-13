using CleanArchitecture.Domain.IRepositories;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class UserVacanciesRepository : IUserVacanciesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserVacanciesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int CountUserVacanciesCreatedToday(string userId)
        {
            var count = _dbContext.UserVacancies
                .AsEnumerable()
                .Where(userVacancy => userVacancy.CreatedDate.GetValueOrDefault().Date == DateTime.Today.Date)
                .Count();

            return count;
        }

    }
}
