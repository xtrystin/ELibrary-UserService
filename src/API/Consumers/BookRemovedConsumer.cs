using ELibrary_UserService.Application.Command;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.Consumers;

public class BookRemovedConsumer : IConsumer<BookDeleted>
{
    private readonly IBookProvider _bookProvider;

    public BookRemovedConsumer(IBookProvider bookProvider)
	{
        _bookProvider = bookProvider;
    }

    public async Task Consume(ConsumeContext<BookDeleted> context)
    {
        var message = context.Message;
        await _bookProvider.RemoveBook(message.BookId);
    }
}
