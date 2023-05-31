namespace ELibrary_UserService.ServiceBus;

public interface IMessagePublisher
{
    Task Publish<T>(T message);
}
