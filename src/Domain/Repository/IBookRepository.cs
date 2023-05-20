using ELibrary_UserService.Domain.Entity;

namespace ELibrary_UserService.Domain.Repository;
public interface IBookRepository
{
    Task AddAsync(Book entity);
    Task DeleteAsync(Book entity);
    Task<bool> Exists(int bookId);
    Task<Book> GetAsync(int id);
}
