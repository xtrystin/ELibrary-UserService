using ELibrary_UserService.Application.Command;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.Consumers;

public class BookCreatedConsumer : IConsumer<BookCreated>
{
    private readonly IBookProvider _bookProvider;

    public BookCreatedConsumer(IBookProvider bookProvider)
    {
        _bookProvider = bookProvider;
    }

    public async Task Consume(ConsumeContext<BookCreated> context)
    {
        var message = context.Message;
        await _bookProvider.CreateBook(message.BookId);
    }
}
