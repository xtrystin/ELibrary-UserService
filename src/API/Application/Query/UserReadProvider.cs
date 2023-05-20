using Dapper;
using ELibrary_UserService.Application.Query.Model;
using ELibrary_UserService.Domain.Entity;
using Npgsql;

namespace ELibrary_UserService.Application.Query;

public class UserReadProvider : IUserReadProvider
{
    private readonly IConfiguration _configuration;

    public UserReadProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<UserReadModel?> GetUserById(string userId)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));

        var sql = @"select u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email"", u.""Description"", u.""AmountToPay"", u.""IsAccountBlocked""
            from ""userService"".""User"" u 
            inner join public.""AspNetUsers"" anu on u.""Id"" = anu.""Id"" 
            where u.""Id"" = @Id
";

        var user = await connection.QueryAsync<UserReadModel>(sql, new { Id = userId });
        return user.FirstOrDefault();
    }

    public async Task<UserReadModel?> GetUserByEmail(string email)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));

        var sql = @"select u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email"", u.""Description"", u.""AmountToPay"", u.""IsAccountBlocked""
            from ""userService"".""User"" u 
            inner join public.""AspNetUsers"" anu on u.""Id"" = anu.""Id"" 
           where anu.""Email"" = @Email
";

        var user = await connection.QueryAsync<UserReadModel>(sql, new { Email = email });
        return user.FirstOrDefault();
    }

    public async Task<List<UserReadModel>> GetUsers()
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));

        var sql = @"select u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email"", u.""Description"", u.""AmountToPay"", u.""IsAccountBlocked""
            from ""userService"".""User"" u 
            inner join public.""AspNetUsers"" anu on u.""Id"" = anu.""Id"" 
";

        var users = await connection.QueryAsync<UserReadModel>(sql);
        return users.ToList();
    }
}
