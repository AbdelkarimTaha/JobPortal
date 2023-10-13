namespace CleanArchitecture.Domain.IRepositories
{
    public interface IUserVacanciesRepository
    {
        int CountUserVacanciesCreatedToday(string userId);
    }
}
