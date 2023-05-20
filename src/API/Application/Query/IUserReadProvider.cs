using ELibrary_UserService.Application.Query.Model;

namespace ELibrary_UserService.Application.Query;

public interface IUserReadProvider
{
    Task<UserReadModel?> GetUserById(string userId);
    Task<UserReadModel?> GetUserByEmail(string email);
    Task<List<UserReadModel>> GetUsers();
}