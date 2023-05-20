using ELibrary_UserService.Application.Command.Exception;
using ELibrary_UserService.Domain.Entity;
using ELibrary_UserService.Domain.Repository;

namespace ELibrary_UserService.Application.Command;

public class BookProvider : IBookProvider
{
    private readonly IBookRepository _bookRepository;

    public BookProvider(IBookRepository bookRepository)
	{
        _bookRepository = bookRepository;
    }

    public async Task CreateBook(int bookId)
    {
        if (await _bookRepository.Exists(bookId))
            throw new AlreadyExistsException("Book already exists");

        var book = new Book(bookId);
        await _bookRepository.AddAsync(book);
    }

    public async Task RemoveBook(int bookId)
    {
        var book = await _bookRepository.GetAsync(bookId);
        if (book == null)
            throw new EntityNotFoundException("Book does not exist");

        await _bookRepository.DeleteAsync(book);
    }
}
