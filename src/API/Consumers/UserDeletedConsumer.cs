using ELibrary_UserService.Application.Command;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.Consumers;

public class UserDeletedConsumer : IConsumer<UserDeleted>
{
    private readonly IUserProvider _userProvider;

    public UserDeletedConsumer(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    public async Task Consume(ConsumeContext<UserDeleted> context)
    {
        var message = context.Message;
        await _userProvider.DeleteUser(message.UserId);
    }
}
