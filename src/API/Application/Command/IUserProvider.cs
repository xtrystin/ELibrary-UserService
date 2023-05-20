using ELibrary_UserService.Application.Command.Model;
using RabbitMqMessages;

namespace ELibrary_UserService.Application.Command;

public interface IUserProvider
{
    Task AddAmounToPay(string userId, decimal amount);
    Task AddToWatchList(string userId, int bookId);
    Task BlockUser(string userId);
    Task CreateUser(UserCreated userData);
    Task DeleteUser(string userId);
    Task ModifyUser(string userId, ModifyUserModel data);
    Task Pay(string userId, decimal amount);
    Task RemoveFromWatchList(string userId, int bookId);
    Task UnBlockUser(string userId);
}
