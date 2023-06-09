﻿using ELibrary_UserService.Application.Command;
using MassTransit;
using ServiceBusMessages;

namespace ELibrary_UserService.Consumers;

public class BookRemovedConsumer : IConsumer<BookRemoved>
{
    private readonly IBookProvider _bookProvider;

    public BookRemovedConsumer(IBookProvider bookProvider)
	{
        _bookProvider = bookProvider;
    }

    public async Task Consume(ConsumeContext<BookRemoved> context)
    {
        var message = context.Message;
        await _bookProvider.RemoveBook(message.BookId);
    }
}

public class BookRemovedUConsumer : IConsumer<BookRemovedU>
{
    private readonly IBookProvider _bookProvider;

    public BookRemovedUConsumer(IBookProvider bookProvider)
	{
        _bookProvider = bookProvider;
    }

    public async Task Consume(ConsumeContext<BookRemovedU> context)
    {
        var message = context.Message;
        await _bookProvider.RemoveBook(message.BookId);
    }
}
