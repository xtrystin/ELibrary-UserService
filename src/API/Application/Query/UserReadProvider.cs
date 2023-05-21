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

        var sql = @"select u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email"", u.""Description"", u.""AmountToPay"", u.""IsAccountBlocked"", b.""Id"" , b.""Title"", b.""ImageUrl"" 
            from ""userService"".""User"" u 
            inner join public.""AspNetUsers"" anu on u.""Id"" = anu.""Id"" 
            left join ""userService"".""BookUser"" bu on u.""Id"" = bu.""UsersId"" 
            left join ""bookService"".""Book"" b on bu.""WatchListId"" = b.""Id"" 
            where u.""Id"" = @Id
";

        var users = await connection.QueryAsync<UserReadModel, BookBasicInfoReadModel,
            (UserReadModel UserReadModel, BookBasicInfoReadModel BookBasicInfoReadModel)>(sql, (user, book) => (user, book), new { Id = userId }, splitOn: "Id");

        var result = users.GroupBy(ub => ub.UserReadModel.Id)
            .Select(g =>
            {
                var user = g.First().UserReadModel;

                user.WatchList = g.Select(ub => ub.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                    .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).ToList();

                return user;
            });

        return result.FirstOrDefault();
    }

    public async Task<UserReadModel?> GetUserByEmail(string email)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));

        var sql = @"select u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email"", u.""Description"", u.""AmountToPay"", u.""IsAccountBlocked"", b.""Id"" , b.""Title"", b.""ImageUrl"" 
            from ""userService"".""User"" u 
            inner join public.""AspNetUsers"" anu on u.""Id"" = anu.""Id"" 
            left join ""userService"".""BookUser"" bu on u.""Id"" = bu.""UsersId"" 
            left join ""bookService"".""Book"" b on bu.""WatchListId"" = b.""Id"" 
            where anu.""Email"" = @Email
";

        var users = await connection.QueryAsync<UserReadModel, BookBasicInfoReadModel,
            (UserReadModel UserReadModel, BookBasicInfoReadModel BookBasicInfoReadModel)>(sql, (user, book) => (user, book), new { Email = email }, splitOn: "Id");

        var result = users.GroupBy(ub => ub.UserReadModel.Id)
            .Select(g =>
            {
                var user = g.First().UserReadModel;

                user.WatchList = g.Select(ub => ub.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                    .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).ToList();

                return user;
            });

        return result.FirstOrDefault();
    }

    public async Task<List<UserReadModel>> GetUsers()
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));

        var sql = @"select u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email"", u.""Description"", u.""AmountToPay"", u.""IsAccountBlocked"", b.""Id"" , b.""Title"", b.""ImageUrl"" 
            from ""userService"".""User"" u 
            inner join public.""AspNetUsers"" anu on u.""Id"" = anu.""Id"" 
            left join ""userService"".""BookUser"" bu on u.""Id"" = bu.""UsersId"" 
            left join ""bookService"".""Book"" b on bu.""WatchListId"" = b.""Id"" 
";

        var users = await connection.QueryAsync<UserReadModel, BookBasicInfoReadModel, 
            (UserReadModel UserReadModel, BookBasicInfoReadModel BookBasicInfoReadModel)>(sql, (user, book) => (user, book), splitOn: "Id");

        var result = users.GroupBy(ub => ub.UserReadModel.Id)
            .Select(g =>
            {
                var user = g.First().UserReadModel;

                user.WatchList = g.Select(ub => ub.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                    .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).ToList();

                return user;
            });

        return result.ToList();
    }
}
