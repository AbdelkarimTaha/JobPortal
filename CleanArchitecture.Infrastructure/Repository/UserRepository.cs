using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserRepository(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }


        public async Task CreateAdminUser()
        {
            var result = await _userManager.FindByNameAsync("admin");
            if (result == null)
            {
                var user = await _userManager.CreateAsync(new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com"
                }, "admin");
                if (user.Succeeded)
                {
                    var admin = await _userManager.FindByNameAsync("admin");
                    await _userManager.AddToRoleAsync(admin, UserRole.Employer.ToString());
                }
            }
        }
    }
}
