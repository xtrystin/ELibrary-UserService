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

        var sql = @"select u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email"", u.""Description"", u.""AmountToPay"", u.""IsAccountBlocked"", b.""Id"" , b.""Title"", b.""ImageUrl"", r.""Id"", r.""BookId"", r.""Like"", rev.""Id"", rev.""BookId"", rev.""Content""
            from ""userService"".""User"" u 
            inner join public.""AspNetUsers"" anu on u.""Id"" = anu.""Id"" 
            left join ""userService"".""BookUser"" bu on u.""Id"" = bu.""UsersId"" 
            left join ""bookService"".""Book"" b on bu.""WatchListId"" = b.""Id"" 
            left join ""userService"".""Reaction"" r on u.""Id"" = r.""UserId"" 
            left join ""userService"".""Review"" rev on u.""Id"" = rev.""UserId"" 
            where u.""Id"" = @Id
";

        var users = await connection.QueryAsync<UserReadModel, BookBasicInfoReadModel, ReactionReadModel, ReviewReadModel,
            (UserReadModel UserReadModel, BookBasicInfoReadModel BookBasicInfoReadModel, ReactionReadModel ReactionReadModel, ReviewReadModel ReviewReadModel)>(sql, (user, book, reaction, review) => (user, book, reaction, review), new { Id = userId }, splitOn: "Id");

        var result = users.GroupBy(ub => ub.UserReadModel.Id)
            .Select(g =>
            {
                var user = g.First().UserReadModel;

                user.WatchList = g.Select(ub => ub.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                    .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).ToList();
                
                user.Reactions = g.Select(ub => ub.ReactionReadModel).Where(x => x != null).GroupBy(x => x.ReactionId)
                    .Select(x => new ReactionReadModel(x.First().ReactionId, x.First().BookId, x.First().Like)).ToList();

                user.Reviews = g.Select(ub => ub.ReviewReadModel).Where(x => x != null).GroupBy(x => x.ReviewId)
                    .Select(x => new ReviewReadModel(x.First().ReviewId, x.First().BookId, x.First().Content)).ToList();

                return user;
            });

        return result.FirstOrDefault();
    }

    public async Task<UserReadModel?> GetUserByEmail(string email)
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));

        var sql = @"select u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email"", u.""Description"", u.""AmountToPay"", u.""IsAccountBlocked"", b.""Id"" , b.""Title"", b.""ImageUrl"", r.""Id"", r.""BookId"", r.""Like"", rev.""Id"", rev.""BookId"", rev.""Content""
            from ""userService"".""User"" u 
            inner join public.""AspNetUsers"" anu on u.""Id"" = anu.""Id"" 
            left join ""userService"".""BookUser"" bu on u.""Id"" = bu.""UsersId"" 
            left join ""bookService"".""Book"" b on bu.""WatchListId"" = b.""Id"" 
            left join ""userService"".""Reaction"" r on u.""Id"" = r.""UserId"" 
            left join ""userService"".""Review"" rev on u.""Id"" = rev.""UserId"" 
            where anu.""Email"" = @Email
";

        var users = await connection.QueryAsync<UserReadModel, BookBasicInfoReadModel, ReactionReadModel, ReviewReadModel,
            (UserReadModel UserReadModel, BookBasicInfoReadModel BookBasicInfoReadModel, ReactionReadModel ReactionReadModel, ReviewReadModel ReviewReadModel)>(sql, (user, book, reaction, review) => (user, book, reaction, review), new { Email = email }, splitOn: "Id");

        var result = users.GroupBy(ub => ub.UserReadModel.Id)
            .Select(g =>
            {
                var user = g.First().UserReadModel;

                user.WatchList = g.Select(ub => ub.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                    .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).ToList();
                
                user.Reactions = g.Select(ub => ub.ReactionReadModel).Where(x => x != null).GroupBy(x => x.ReactionId)
                    .Select(x => new ReactionReadModel(x.First().ReactionId, x.First().BookId, x.First().Like)).ToList();

                user.Reviews = g.Select(ub => ub.ReviewReadModel).Where(x => x != null).GroupBy(x => x.ReviewId)
                    .Select(x => new ReviewReadModel(x.First().ReviewId, x.First().BookId, x.First().Content)).ToList();

                return user;
            });

        return result.FirstOrDefault();
    }

    public async Task<List<UserReadModel>> GetUsers()
    {
        using var connection = new NpgsqlConnection(_configuration.GetConnectionString("PostgresResourceDb"));

        var sql = @"select u.""Id"", u.""FirstName"", u.""LastName"", anu.""Email"", u.""Description"", u.""AmountToPay"", u.""IsAccountBlocked"", b.""Id"" , b.""Title"", b.""ImageUrl"", r.""Id"", r.""BookId"", r.""Like"", rev.""Id"", rev.""BookId"", rev.""Content""
            from ""userService"".""User"" u 
            inner join public.""AspNetUsers"" anu on u.""Id"" = anu.""Id"" 
            left join ""userService"".""BookUser"" bu on u.""Id"" = bu.""UsersId"" 
            left join ""bookService"".""Book"" b on bu.""WatchListId"" = b.""Id"" 
            left join ""userService"".""Reaction"" r on u.""Id"" = r.""UserId"" 
            left join ""userService"".""Review"" rev on u.""Id"" = rev.""UserId"" 
";

        var users = await connection.QueryAsync<UserReadModel, BookBasicInfoReadModel, ReactionReadModel, ReviewReadModel,
            (UserReadModel UserReadModel, BookBasicInfoReadModel BookBasicInfoReadModel, ReactionReadModel ReactionReadModel, ReviewReadModel ReviewReadModel)> (sql, (user, book, reaction, review) => (user, book, reaction, review), splitOn: "Id");

        var result = users.GroupBy(ub => ub.UserReadModel.Id)
            .Select(g =>
            {
                var user = g.First().UserReadModel;

                user.WatchList = g.Select(ub => ub.BookBasicInfoReadModel).Where(x => x != null).GroupBy(x => x.Id)
                    .Select(x => new BookBasicInfoReadModel(x.First().Id, x.First().Title, x.First().ImageUrl)).ToList();

                user.Reactions = g.Select(ub => ub.ReactionReadModel).Where(x => x != null).GroupBy(x => x.ReactionId)
                    .Select(x => new ReactionReadModel(x.First().ReactionId, x.First().BookId, x.First().Like)).ToList();

                user.Reviews = g.Select(ub => ub.ReviewReadModel).Where(x => x != null).GroupBy(x => x.ReviewId)
                    .Select(x => new ReviewReadModel(x.First().ReviewId, x.First().BookId, x.First().Content)).ToList();

                return user;
            });

        return result.ToList();
    }
}
