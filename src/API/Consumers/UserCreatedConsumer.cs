using ELibrary_UserService.Application.Command;
using ELibrary_UserService.Domain.Entity;
using ELibrary_UserService.Domain.Repository;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly IUserRepository _userRepository;

    public UserCreatedConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var message = context.Message;
        var user = await _userRepository.GetAsync(message.UserId);
        if (user is null)
        {
            user = new User(message.UserId, message.FirstName, message.LastName);
            await _userRepository.AddAsync(user);
        }
    }
}

public class UserCreatedUConsumer : IConsumer<UserCreatedU>
{
    private readonly IUserRepository _userRepository;

    public UserCreatedUConsumer(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Consume(ConsumeContext<UserCreatedU> context)
    {
        var message = context.Message;
        var user = await _userRepository.GetAsync(message.UserId);
        if (user is null)
        {
            user = new User(message.UserId, message.FirstName, message.LastName);
            await _userRepository.AddAsync(user);
        }
    }
}
