using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.IServices;
using CleanArchitecture.Domain.Models;
using System.Security.Claims;

namespace CleanArchitecture.Application.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository;

        public VacancyService(IVacancyRepository vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
        }

        public async Task<bool> Create(VacancyPostDto vacancyDto)
        {
            var vacancy = Vacancy.Create(vacancyDto);
            await _vacancyRepository.Create(vacancy);
            return true;    
        }

        public async Task<bool> Delete(Guid id)
        {
            var vacancy = await _vacancyRepository.GetById(id);

            if (vacancy == null)
                return false;

            vacancy.UpdateIsActive(false);
            await _vacancyRepository.Update(vacancy);
            return true;
        }

        public async Task<IEnumerable<VacancyGetDto>> GetAll()
        {
            var vacancies = await _vacancyRepository.GetAll();
            return Vacancy.ConvertEntityToModel(vacancies);
        }

        public async Task<VacancyGetDto> GetById(Guid id)
        {
            var vacancy = await _vacancyRepository.GetById(id);
           
            if (vacancy == null)
                return null;

            return Vacancy.ConvertEntityToModel(vacancy);
        }

        public async Task<bool> Post(Guid id)
        {
            var vacancy = await _vacancyRepository.GetById(id);

            if (vacancy == null)
                return false;

            vacancy.UpdateStatus(VacancyStatus.Active);
            await _vacancyRepository.Update(vacancy);
            return true;
        }
        
        public async Task<bool> DeActivate(Guid id)
        {
            var vacancy = await _vacancyRepository.GetById(id);

            if (vacancy == null)
                return false;

            vacancy.UpdateStatus(VacancyStatus.DeActivate);
            await _vacancyRepository.Update(vacancy);
            return true;
        }

        public async Task<bool> Update(VacancyPutDto model)
        {
            var vacancy = await _vacancyRepository.GetById(model.Id);

            if (vacancy == null)
                return false;

            vacancy.Update(model);
            await _vacancyRepository.Update(vacancy);
            return true;
        }
    }
}
