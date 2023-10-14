using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.IServices;

namespace CleanArchitecture.Application.Services
{
    public class RecurringJobService : IRecurringJobService
    {
        private readonly IVacancyRepository _vacancyRepository;

        public RecurringJobService(IVacancyRepository vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
        }
        public void ArchivingExpiredVacancies()
        {
            try
            {
                var vacancies = _vacancyRepository.GetExpiredVacancies();

                if (!vacancies.Any())
                    return;

                vacancies.ForEach(_ =>
                {
                    _.UpdateStatus((int)VacancyStatus.Expired);
                    _vacancyRepository.Update(_);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error archiving expired vacancies: {ex.Message}");
            }
        }
    }
}
