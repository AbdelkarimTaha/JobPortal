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
        Task<bool> UpdateStatus(VacancyStatusDto VacancyStatusDto);
        Task<List<VacancyGetDto>> Search(VacancySearchDto vacancySearchDto);
        Task<string> Apply(Guid id, string? currentUser);
        Task<List<VacancyApplicantGetDto>> VacancyApplicantsList(Guid id);
    }
}
