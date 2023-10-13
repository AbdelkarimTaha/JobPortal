using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.IServices;
using CleanArchitecture.Domain.Models;
using System.Collections.Generic;

namespace CleanArchitecture.Application.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserVacanciesRepository _userVacanciesRepository;

        public VacancyService(IVacancyRepository vacancyRepository, IUserRepository userRepository, IUserVacanciesRepository userVacanciesRepository)
        {
            _vacancyRepository = vacancyRepository;
            _userRepository = userRepository;
            _userVacanciesRepository = userVacanciesRepository;
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

        public async Task<bool> UpdateStatus(VacancyStatusDto vacancyStatusDto)
        {
            var vacancy = await _vacancyRepository.GetById(vacancyStatusDto?.Id ?? new Guid());

            if (vacancy == null)
                return false;

            vacancy.UpdateStatus(vacancyStatusDto?.Status);
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

        public async Task<List<VacancyGetDto>> Search(VacancySearchDto vacancySearchDto)
        {
            var vacancies = await _vacancyRepository.SearchAsync(vacancySearchDto);

            var vacancyDtos = vacancies
                .Select(_ => Vacancy.ConvertEntityToModel(_))
                .ToList();

            return vacancyDtos;
        }

        public async Task<string> Apply(Guid id, string? currentUser)
        {
            string userId = _userRepository.GetIdByName(currentUser);
            var userVacancy = UserVacancies.Create(id, currentUser, userId);
            var vacancy = await _vacancyRepository.GetById(id);

            var validationMessage = Validation(vacancy, userId);
            if (!string.IsNullOrWhiteSpace(validationMessage))
                return validationMessage;

            vacancy.UserVacancies.Add(userVacancy);
            vacancy.IncrementTotalApplications();
            await _vacancyRepository.Update(vacancy);
            return "Request applied successfully";
        }

        private string Validation(Vacancy vacancy, string userId)
        {
            if (vacancy == null)
                return "Vacancy not found.";

            if (vacancy.MaxApplications <= vacancy.TotalApplications)
                return "Maximum applications for this vacancy reached.";

            if (_userVacanciesRepository.CountUserVacanciesCreatedToday(userId) >= 1)
                return "Not allowed to apply for more than one vacancy per day (24 hours).";

            return string.Empty;
        }

        public async Task<List<VacancyApplicantGetDto>> VacancyApplicantsList(Guid id)
        {
            var vacancy = await _vacancyRepository.GetById(id);

            if (vacancy == null)
                return null;

            var list = vacancy.UserVacancies
                .Select(_ => Vacancy.ConvertEntityToModel(_))
                .ToList();

            return list;
        }
    }
}
