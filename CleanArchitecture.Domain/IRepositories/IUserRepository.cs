namespace CleanArchitecture.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task CreateAdminUser();
        Task<IList<string>> GetUserRoleByUserNameAsync(string userName);
        string GetIdByName(string? userName);
    }
}
