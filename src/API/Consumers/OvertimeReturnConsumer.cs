using ELibrary_UserService.Application.Command;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.Consumers;

public class OvertimeReturnConsumer : IConsumer<OvertimeReturn>
{
    private readonly IUserProvider _userProvider;

    public OvertimeReturnConsumer(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    public async Task Consume(ConsumeContext<OvertimeReturn> context)
    {
        var message = context.Message;
        await _userProvider.AddAmounToPay(message.UserId, message.AmountToPay);
    }
}

public class OvertimeReturnUConsumer : IConsumer<OvertimeReturnU>
{
    private readonly IUserProvider _userProvider;

    public OvertimeReturnUConsumer(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }

    public async Task Consume(ConsumeContext<OvertimeReturnU> context)
    {
        var message = context.Message;
        await _userProvider.AddAmounToPay(message.UserId, message.AmountToPay);
    }
}
