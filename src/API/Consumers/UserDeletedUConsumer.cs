using ELibrary_UserService.Application.Command;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.Consumers;

public class UserDeletedUConsumer : IConsumer<UserDeletedU>
{
    private readonly IUserProvider _userProvider;

    public UserDeletedUConsumer(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    public async Task Consume(ConsumeContext<UserDeletedU> context)
    {
        var message = context.Message;
        await _userProvider.DeleteUser(message.UserId);
    }
}
