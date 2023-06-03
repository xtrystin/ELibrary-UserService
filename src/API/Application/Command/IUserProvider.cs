using ELibrary_UserService.Application.Command.Model;
using ServiceBusMessages;

namespace ELibrary_UserService.Application.Command;

public interface IUserProvider
{
    Task AddAmounToPay(string userId, decimal amount);
    Task AddOrModifyReaction(string userId, int bookId, bool like);
    Task AddOrModifyReview(string userId, int bookId, string content);
    Task AddToWatchList(string userId, int bookId);
    Task BlockUser(string userId);
    Task CreateUser(UserCreated userData);
    Task DeleteUser(string userId);
    Task ModifyUser(string userId, ModifyUserModel data);
    Task Pay(string userId, decimal amount);
    Task RemoveFromWatchList(string userId, int bookId);
    Task RemoveReaction(string userId, int bookId);
    Task RemoveReview(string userId, int bookId);
    Task UnBlockUser(string userId);
}
