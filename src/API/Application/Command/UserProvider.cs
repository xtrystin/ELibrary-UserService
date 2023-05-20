using ELibrary_UserService.Application.Command.Exception;
using ELibrary_UserService.Application.Command.Model;
using ELibrary_UserService.Domain.Entity;
using ELibrary_UserService.Domain.Repository;
using RabbitMqMessages;
using MassTransit;
using ELibrary_UserService.Domain.Exception;
using ELibrary_UserService.RabbitMq.Messages;

namespace ELibrary_UserService.Application.Command;

public class UserProvider : IUserProvider
{
    private readonly IUserRepository _userRepository;
    private readonly IBus _bus;
    private readonly IBookRepository _bookRepository;

    public UserProvider(IUserRepository userRepository, IBus bus, 
        IBookRepository bookRepository)
    {
        _userRepository = userRepository;
        _bus = bus;
        _bookRepository = bookRepository;
    }

    public async Task ModifyUser(string userId, ModifyUserModel data)
    {
        var user = await GetUserAsyncOrThrow(userId);

        user.Modify(data.FirstName, data.LastName, data.Description);
        await _userRepository.UpdateAsync(user);
    }

    public async Task CreateUser(UserCreated userData)
    {
        if (await _userRepository.Exists(userData.UserId))
            throw new Exception.AlreadyExistsException("This user already exists");

        var user = new User(userData.UserId, userData.FirstName, userData.LastName);
        await _userRepository.AddAsync(user);
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
        catch (UserBlockedException ex)
        {
            var message = new UserBlocked() { UserId = userId };
            await _bus.Publish(message);
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
        // await _bus.Publish(message);

        await _userRepository.UpdateAsync(user);
    }

    public async Task UnBlockUser(string userId)
    {
        var user = await GetUserAsyncOrThrow(userId);
        user.UnBlock();

        var message = new UserUnblocked() { UserId = userId };
        //await _bus.Publish(message);

        await _userRepository.UpdateAsync(user);
    }

    private async Task<User> GetUserAsyncOrThrow(string userId)
    {
        var user = await _userRepository.GetAsync(userId);
        if (user is null)
            throw new EntityNotFoundException("User has not been found");
        return user;
    }
}
