using ELibrary_UserService.Domain.Entity;
using ELibrary_UserService.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace ELibrary_UserService.Infrastructure.EF.Repository;
internal class UserRepository : IUserRepository
{
    private readonly UserDbContext _dbContext;

    public UserRepository(UserDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(User entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User entity)
    {
        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetAsync(string userId)
        => await _dbContext.Users.Include(x => x.Reactions).Include(x => x.Reviews).FirstOrDefaultAsync(x => x.Id == userId);

    public async Task UpdateAsync(User entity)
    {
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public Task<bool> Exists(string userId)
        => _dbContext.Users.AnyAsync(x => x.Id == userId);
}
