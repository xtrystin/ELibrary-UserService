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
            await _bus.Send(message);   // send to one queue
        }
    }
}
