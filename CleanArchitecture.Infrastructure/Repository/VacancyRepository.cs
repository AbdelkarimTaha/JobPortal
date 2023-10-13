using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.Models;
using System.Linq;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class VacancyRepository : IVacancyRepository
    {
        private readonly ApplicationDbContext _context;
        public VacancyRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task Create(Vacancy vacancy)
        {
            await _context.Vacancies.AddAsync(vacancy);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var vacancy = _context.Vacancies.Find(id);
            _context.Vacancies.Remove(vacancy);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vacancy>> GetAll()
        {
            return _context.Vacancies
                .Where(_ => _.IsActive == true);
        }

        public async Task<Vacancy> GetById(Guid id)
        {
            return await _context.Vacancies.FindAsync(id);
        }

        public async Task Update(Vacancy vacancy)
        {
            _context.Vacancies.Update(vacancy);
            await _context.SaveChangesAsync();
        }
    }
}