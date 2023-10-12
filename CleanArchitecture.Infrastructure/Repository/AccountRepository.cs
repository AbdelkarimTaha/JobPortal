using CleanArchitecture.Domain.DTOs;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.IRepositories;
using CleanArchitecture.Domain.IServices;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ITokenService _tokenService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;

        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        public AccountRepository(ITokenService tokenService,
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IPasswordHasher<IdentityUser> passwordHasher)
        {
            _tokenService = tokenService;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginDTO> Login(LoginModel model)
        {
            var isAuthenticated = await AuthenticateUser(model);
            if (isAuthenticated)
            {
                var token = GenerateToken(model);
                return new LoginDTO()
                {
                    Username = model.Username,
                    Token = token
                };
            }

            return null;
        }

        public async Task<bool> Register(RegisterModel model)
        {
            await EnsureDefaultRolesExist();
            await EnsureAdminUserExists();

            return await RegisterNewUser(model);
        }



        private async Task<bool> AuthenticateUser(LoginModel model)
        {
            var result = await PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
            return result.Succeeded;
        }

        private string GenerateToken(LoginModel model)
        {
            return _tokenService.CreateToken(model);
        }

        private async Task EnsureDefaultRolesExist()
        {
            await _roleRepository.AddRoles();
        }

        private async Task EnsureAdminUserExists()
        {
            await _userRepository.CreateAdminUser();
        }

        private async Task<bool> RegisterNewUser(RegisterModel model)
        {
            var newUser = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                await AssignUserRole(newUser, model.IsEmployer);
                return true;
            }

            return false;
        }

        private async Task AssignUserRole(IdentityUser user, bool isEmployer)
        {
            if (isEmployer)
            {
                await _userManager.AddToRoleAsync(user, UserRole.Employer.ToString());
            }
            else
            {
                await _userManager.AddToRoleAsync(user, UserRole.Applicant.ToString());
            }
        }

        private async Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            if (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user))
            {
                return SignInResult.LockedOut;
            }

            if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Success)
            {
                if (_userManager.SupportsUserLockout)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }

                return SignInResult.Success;
            }
            else
            {
                if (_userManager.SupportsUserLockout && lockoutOnFailure)
                {
                    await _userManager.AccessFailedAsync(user);
                    if (await _userManager.IsLockedOutAsync(user))
                    {
                        return SignInResult.LockedOut;
                    }
                }
                return SignInResult.Failed;
            }
        }
    }
}