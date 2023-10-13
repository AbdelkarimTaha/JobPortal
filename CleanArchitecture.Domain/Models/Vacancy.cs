using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Domain.Models
{
    public class Vacancy : BaseEntity
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Field { get; private set; }
        public string Requirements { get; private set; }
        public int? Location { get; private set; }
        public int? VacancyTypeId { get; private set; }
        public int? StatusId { get; private set; } = (int)VacancyStatus.Draft;
        public DateTime? ExpiryDate { get; private set; }
        public int? TotalApplications { get; private set; } = 0;
        public int? MaxApplications { get; private set; }
        private Vacancy()
        {

        }
        private Vacancy(VacancyPostDto vacancyDto)
        {
            Id = Guid.NewGuid();
            Title = vacancyDto.Title;
            Field = vacancyDto.Field; ;
            Requirements = vacancyDto.Requirements;
            Location = vacancyDto.Location;
            VacancyTypeId = vacancyDto.VacancyType;
            ExpiryDate = vacancyDto.ExpiryDate;
            MaxApplications = vacancyDto.MaxApplications;
            CreatedDate = DateTime.Now;
        }

        public static Vacancy Create(VacancyPostDto vacancyDto)
        {
            return new Vacancy(vacancyDto);
        }

        public static IEnumerable<VacancyGetDto> ConvertEntityToModel(IEnumerable<Vacancy> vacancies)
        {
            var result = new List<VacancyGetDto>();
            foreach (var vacancy in vacancies)
            {
                result.Add(new()
                {
                    Id = vacancy.Id,
                    CreatedByUser = "",
                    CreatedDate = vacancy.CreatedDate,
                    ExpiryDate = vacancy.ExpiryDate,
                    Field = vacancy.Field,
                    IsActive = vacancy.IsActive,
                    Location = GetEnumDescription((WorkLocation)vacancy.Location),
                    MaxApplications = vacancy.MaxApplications,
                    ModifiedByUser = "",
                    ModifiedDate = vacancy.ModifiedDate,
                    Requirements = vacancy.Requirements,
                    Status = GetEnumDescription((VacancyStatus)vacancy.StatusId),
                    Title = vacancy.Title,
                    TotalApplications = vacancy.TotalApplications,
                    VacancyType = GetEnumDescription((VacancyType)vacancy.VacancyTypeId)
                });

            }
            return result;
        }

        public void UpdateStatus(VacancyStatus status)
        {
            StatusId = (int)status;
        }

        public static VacancyGetDto ConvertEntityToModel(Vacancy vacancy)
        {
            return new()
            {
                Id = vacancy.Id,
                CreatedByUser = "",
                CreatedDate = vacancy.CreatedDate,
                ExpiryDate = vacancy.ExpiryDate,
                Field = vacancy.Field,
                IsActive = vacancy.IsActive,
                Location = GetEnumDescription((WorkLocation)vacancy.Location),
                MaxApplications = vacancy.MaxApplications,
                ModifiedByUser = "",
                ModifiedDate = vacancy.ModifiedDate,
                Requirements = vacancy.Requirements,
                Status = GetEnumDescription((VacancyStatus)vacancy.StatusId),
                Title = vacancy.Title,
                TotalApplications = vacancy.TotalApplications,
                VacancyType = GetEnumDescription((VacancyType)vacancy.VacancyTypeId)
            };
        }

        public void UpdateIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        public void Update(VacancyPutDto vacancyDto)
        {
            Title = vacancyDto.Title;
            Field = vacancyDto.Field; ;
            Requirements = vacancyDto.Requirements;
            Location = vacancyDto.Location.Value;
            VacancyTypeId = vacancyDto.VacancyType.Value;
            ExpiryDate = vacancyDto.ExpiryDate;
            MaxApplications = vacancyDto.MaxApplications;
            ModifiedDate = DateTime.Now;
        }
        private static string GetEnumDescription(Enum enumValue)
        {
            return Enum.GetName(enumValue.GetType(), enumValue);
        }
    }
}
