using ELibrary_UserService.Application.Command;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreated>
{
    private readonly IUserProvider _userProvider;

    public UserCreatedConsumer(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var message = context.Message;
        await _userProvider.CreateUser(message);
    }
}
