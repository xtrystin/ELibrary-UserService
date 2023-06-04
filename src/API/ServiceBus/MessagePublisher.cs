using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.ServiceBus;

public class MessagePublisher : IMessagePublisher
{
    private readonly IBus _bus;
    private readonly IConfiguration _configuration;

    public MessagePublisher(IBus bus, IConfiguration configuration)
    {
        _bus = bus;
        _configuration = configuration;
    }

    public async Task Publish<T>(T message)
    {
        if (_configuration["Flags:UserRabbitMq"] == "1")
        {
            await _bus.Publish(message);
        }
        else
        {
            // Publisg to many queues -> because Basic Tier ASB allowed only 1-1 queues, no topics
            if (message is UserBlocked)
            {
                var m = message as UserBlocked;
                var borrowingServiceMessage = new UserBlockedBr() { UserId = m.UserId };

                await _bus.Send(borrowingServiceMessage);
            }
            else if (message is UserUnblocked)
            {
                var m = message as UserUnblocked;
                var userServiceMessage = new UserUnblockedBr() { UserId = m.UserId };

                await _bus.Send(userServiceMessage);
            }
            else
            {
                // send to one queue
                await _bus.Send(message);
            }
        }
    }
}
