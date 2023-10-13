using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
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

        public string GetIdByName(string? userName)
        {
            return _context.Users.FirstOrDefault(_ => _.UserName == userName)?.Id;
        }

        public async Task<IList<string>> GetUserRoleByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return roles;
            }

            return null; 
        }
    }
}
