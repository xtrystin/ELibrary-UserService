using ELibrary_UserService.Domain.Entity;
using ELibrary_UserService.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ELibrary_UserService.Infrastructure.EF.Repository;
internal class BookRepository : IBookRepository
{
    private readonly UserDbContext _dbContext;

    public BookRepository(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Book entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Book entity)
    {
        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task<bool> Exists(int bookId)
        => _dbContext.Books.AnyAsync(x => x.Id == bookId);

    public async Task<Book> GetAsync(int id)
        => await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
}
