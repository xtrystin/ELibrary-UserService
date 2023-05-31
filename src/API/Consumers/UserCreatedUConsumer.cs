using ELibrary_UserService.Application.Command;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.Consumers;

public class UserCreatedUConsumer : IConsumer<UserCreatedU>
{
    private readonly IUserProvider _userProvider;

    public UserCreatedUConsumer(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    public async Task Consume(ConsumeContext<UserCreatedU> context)
    {
        var message = context.Message;
        UserCreated mappedMessage = new() { UserId = message.UserId, FirstName = message.FirstName, LastName = message.LastName };
        await _userProvider.CreateUser(mappedMessage);
    }
}
