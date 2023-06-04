using ELibrary_UserService.Application.Command.Exception;
using ELibrary_UserService.Application.Command.Model;
using ELibrary_UserService.Domain.Entity;
using ELibrary_UserService.Domain.Exception;
using ELibrary_UserService.Domain.Repository;
using ELibrary_UserService.ServiceBus;
using ServiceBusMessages;

namespace ELibrary_UserService.Application.Command;

public class UserProvider : IUserProvider
{
    private readonly IUserRepository _userRepository;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IBookRepository _bookRepository;

    public UserProvider(IUserRepository userRepository, IMessagePublisher messagePublisher, 
        IBookRepository bookRepository)
    {
        _userRepository = userRepository;
        _messagePublisher = messagePublisher;
        _bookRepository = bookRepository;
    }

    public async Task ModifyUser(string userId, ModifyUserModel data)
    {
        var user = await GetUserAsyncOrThrow(userId);

        user.Modify(data.FirstName, data.LastName, data.Description);
        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUser(string userId)
    {
        var user = await GetUserAsyncOrThrow(userId);
        await _userRepository.DeleteAsync(user);
    }

    public async Task AddAmounToPay(string userId, decimal amount)
    {
        var user = await GetUserAsyncOrThrow(userId);

        try
        {
            user.AddAmountToPay(amount);
        }
        catch (UserBlockedException)
        {
            var message = new UserBlocked() { UserId = userId };
            await _messagePublisher.Publish(message);
        }

        await _userRepository.UpdateAsync(user);
    }

    public async Task Pay(string userId, decimal amount)
    {
        var user = await GetUserAsyncOrThrow(userId);
        user.Pay(amount);

        _userRepository.UpdateAsync(user);
    }

    public async Task AddToWatchList(string userId, int bookId)
    {
        var user = await GetUserAsyncOrThrow(userId);
        var book = await _bookRepository.GetAsync(bookId);
        if (book is null)
            throw new EntityNotFoundException("Book has not been found");
        
        user.AddToWatchList(book);
        await _userRepository.UpdateAsync(user);
    }

    public async Task RemoveFromWatchList(string userId, int bookId)
    {
        var user = await GetUserAsyncOrThrow(userId);
        var book = await _bookRepository.GetAsync(bookId);
        if (book is null)
            throw new EntityNotFoundException("Book has not been found");

        user.RemoveFromWatchList(book);
        await _userRepository.UpdateAsync(user);
    }

    public async Task BlockUser(string userId)
    {
        var user = await GetUserAsyncOrThrow(userId);
        user.Block();

        var message = new UserBlocked() { UserId = userId };
        await _messagePublisher.Publish(message);

        await _userRepository.UpdateAsync(user);
    }

    public async Task UnBlockUser(string userId)
    {
        var user = await GetUserAsyncOrThrow(userId);
        user.UnBlock();

        var message = new UserUnblocked() { UserId = userId };
        await _messagePublisher.Publish(message);

        await _userRepository.UpdateAsync(user);
    }

    public async Task AddOrModifyReaction(string userId, int bookId, bool like)
    {
        var user = await GetUserAsyncOrThrow(userId);
        var book = await GetBookOrThrow(bookId);
        user.AddOrModifyReaction(book, like);

        await _userRepository.UpdateAsync(user);
    }

    public async Task RemoveReaction(string userId, int bookId)
    {
        var user = await GetUserAsyncOrThrow(userId);
        var book = await GetBookOrThrow(bookId);
        user.RemoveReaction(book);

        await _userRepository.UpdateAsync(user);
    }

    public async Task AddOrModifyReview(string userId, int bookId, string content)
    {
        var user = await GetUserAsyncOrThrow(userId);
        var book = await GetBookOrThrow(bookId);
        user.AddOrModifyReview(book, content);

        await _userRepository.UpdateAsync(user);
    }

    public async Task RemoveReview(string userId, int bookId)
    {
        var user = await GetUserAsyncOrThrow(userId);
        var book = await GetBookOrThrow(bookId);
        user.RemoveReview(book);

        await _userRepository.UpdateAsync(user);
    }

    private async Task<User> GetUserAsyncOrThrow(string userId)
    {
        var user = await _userRepository.GetAsync(userId);
        if (user is null)
            throw new EntityNotFoundException("User has not been found");
        return user;
    }

    private async Task<Book> GetBookOrThrow(int bookId)
    {
        var book = await _bookRepository.GetAsync(bookId);
        if (book == null)
            throw new EntityNotFoundException("Book does not exist");
        return book;
    }
}
