using ELibrary_UserService.Domain.Entity;

namespace ELibrary_UserService.Domain.Repository;
public interface IUserRepository
{
    Task AddAsync(User entity);
    Task DeleteAsync(User entity);
    Task<bool> Exists(string userId);
    Task<User> GetAsync(string userId);
    Task UpdateAsync(User entity);
}
