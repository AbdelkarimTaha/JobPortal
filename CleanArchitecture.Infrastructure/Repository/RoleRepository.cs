using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task AddRoles()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = UserRole.Employer.ToString(), NormalizedName = UserRole.Employer.ToString().ToUpper() });
                await _roleManager.CreateAsync(new IdentityRole { Name = UserRole.Applicant.ToString(), NormalizedName = UserRole.Applicant.ToString().ToUpper() });
            }
        }
    }
}
