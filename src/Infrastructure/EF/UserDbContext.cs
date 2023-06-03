using ELibrary_UserService.Domain.Entity;
using ELibrary_UserService.Infrastructure.EF.Config;
using Microsoft.EntityFrameworkCore;

namespace ELibrary_UserService.Infrastructure.EF;
public class UserDbContext : DbContext
{
    public const string DEFAULT_SCHEMA = "userService";
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }

	public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(DEFAULT_SCHEMA);

		modelBuilder.ApplyConfiguration(new UserEntityTypeConfig());
		modelBuilder.ApplyConfiguration(new ReviewTypeConfig());
		modelBuilder.ApplyConfiguration(new ReactionTypeConfig());
		modelBuilder.Entity<Book>().ToTable("Book");
		
	}
}
