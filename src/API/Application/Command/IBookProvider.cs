namespace ELibrary_UserService.Application.Command;

public interface IBookProvider
{
    Task CreateBook(int bookId);
    Task RemoveBook(int bookId);
}