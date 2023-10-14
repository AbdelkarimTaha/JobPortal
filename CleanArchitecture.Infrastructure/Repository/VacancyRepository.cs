using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.Models;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Vacancies
                .Where(v => v.Id == id)
                .Include(v => v.UserVacancies)
                .FirstOrDefaultAsync();
        }

        public List<Vacancy> GetExpiredVacancies()
        {
            var vacancies = _context.Vacancies
                .AsEnumerable()
                .Where(_ => _.IsActive == true && _.StatusId == (int)VacancyStatus.Active && _.ExpiryDate.GetValueOrDefault().Date < DateTime.Today.Date)
                .ToList();

            return vacancies;
        }

        public async Task<List<Vacancy>> SearchAsync(VacancySearchDto vacancySearchDto)
        {
            var query = _context.Vacancies.AsQueryable();

            if (vacancySearchDto.Id != null)
            {
                query = query.Where(v => v.Id == vacancySearchDto.Id);
            }

            if (!string.IsNullOrWhiteSpace(vacancySearchDto.Title))
            {
                query = query.Where(v => v.Title.ToLower().Contains(vacancySearchDto.Title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(vacancySearchDto.Field))
            {
                query = query.Where(v => v.Field.ToLower().Contains(vacancySearchDto.Field.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(vacancySearchDto.Requirements))
            {
                query = query.Where(v => v.Requirements.ToLower().Contains(vacancySearchDto.Requirements.ToLower()));
            }

            if (vacancySearchDto.Location != null)
            {
                query = query.Where(v => v.Location == vacancySearchDto.Location);
            }

            if (vacancySearchDto.VacancyType != null)
            {
                query = query.Where(v => v.VacancyTypeId == vacancySearchDto.VacancyType);
            }

            if (vacancySearchDto.Status != null)
            {
                query = query.Where(v => v.StatusId == vacancySearchDto.Status);
            }

            if (vacancySearchDto.ExpiryDate != null)
            {
                query = query.Where(v => v.ExpiryDate == vacancySearchDto.ExpiryDate);
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Update(Vacancy vacancy)
        {
            _context.Vacancies.Update(vacancy);
            await _context.SaveChangesAsync();
        }
    }
}