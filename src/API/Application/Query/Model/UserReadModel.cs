namespace ELibrary_UserService.Application.Query.Model;

public class UserReadModel
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Description { get; set; }
    public decimal AmountToPay { get; set; }
    public bool IsAccountBlocked { get; set; }

}
