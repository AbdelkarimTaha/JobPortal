using CleanArchitecture.Domain.DTOs;

namespace CleanArchitecture.Domain.IServices
{
    public interface IVacancyService
    {
        Task<VacancyGetDto> GetById(Guid id);
        Task<bool> Delete(Guid id);
        Task<bool> Update(VacancyPutDto model);
        Task<bool> Create(VacancyPostDto model);
        Task<IEnumerable<VacancyGetDto>> GetAll();
        Task<bool> Post(Guid id);
        Task<bool> DeActivate(Guid id);
    }
}
