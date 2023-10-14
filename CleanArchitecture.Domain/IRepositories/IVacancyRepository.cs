using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.Models;

namespace CleanArchitecture.Domain.IRepositories
{
    public interface IVacancyRepository
    {
        public Task Create(Vacancy vacancy);
        public Task Update(Vacancy vacancy);
        public Task Delete(Guid id);
        public Task<Vacancy> GetById(Guid id);
        public Task<IEnumerable<Vacancy>> GetAll();
        Task<List<Vacancy>> SearchAsync(VacancySearchDto vacancySearchDto);
        List<Vacancy> GetExpiredVacancies();
    }
}
