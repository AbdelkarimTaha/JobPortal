using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CleanArchitecture.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class VacancyController : ControllerBase
    {
        private readonly IVacancyService _vacancyService;

        public VacancyController(IVacancyService vacancyService)
        {
            _vacancyService = vacancyService;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(VacancyPostDto model)
        {
            if (ModelState.IsValid)
            {
                var created = await _vacancyService.Create(model);
                if (created)
                {
                    return Ok(new { Message = "Vacancy created successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to create vacancy" });
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(VacancyPutDto model)
        {
            if (ModelState.IsValid)
            {
                var updated = await _vacancyService.Update(model);
                if (updated)
                {
                    return Ok(new { Message = "Vacancy updated successfully" });
                }
                else
                {
                    return BadRequest(new { Message = "Failed to update vacancy" });
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vacancy = await _vacancyService.GetById(id);
            if (vacancy != null)
            {
                return Ok(vacancy);
            }
            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var vacancies = await _vacancyService.GetAll();
            if (vacancies != null)
            {
                return Ok(vacancies);
            }
            return NotFound();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _vacancyService.Delete(id);
            if (deleted)
            {
                return Ok(new { Message = "Vacancy deleted successfully" });
            }
            else
            {
                return NotFound(new { Message = "Vacancy not found or deletion failed" });
            }
        }

        [HttpPut("Post/{id}")]
        public async Task<IActionResult> Post(Guid id)
        {
            var posted = await _vacancyService.Post(id);
            if (posted)
            {
                return Ok(new { Message = "Vacancy posted successfully" });
            }
            else
            {
                return NotFound(new { Message = "Vacancy not found or Posted failed" });
            }
        }

        [HttpPut("DeActivate/{id}")]
        public async Task<IActionResult> DeActivate(Guid id)
        {
            var deActivated = await _vacancyService.DeActivate(id);
            if (deActivated)
            {
                return Ok(new { Message = "Vacancy deactivated successfully" });
            }
            else
            {
                return NotFound(new { Message = "Vacancy not found or deactivated failed" });
            }
        }

        //[HttpPost("Search")]
        //public async Task<IActionResult> Search()
        //{
        //    var vacancy = await _vacancyService.Search();
        //    if (vacancy != null)
        //    {
        //        return Ok(vacancy);
        //    }
        //    return NotFound();
        //}
        
        //[HttpPost("Apply")]
        //public async Task<IActionResult> Apply()
        //{
        //    var vacancy = await _vacancyService.Apply();
        //    if (vacancy != null)
        //    {
        //        return Ok(vacancy);
        //    }
        //    return NotFound();
        //}




        [HttpGet("GetCurrentUser")]
        public IActionResult GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userName = User.Identity.Name;
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

                return Ok(new
                {
                    UserId = userId,
                    UserName = userName,
                    UserEmail = userEmail,
                    IsAdmin = User.IsInRole("Admin")
                });
            }
            else
            {
                return Unauthorized(); // User is not authenticated
            }
        }
    }
}

