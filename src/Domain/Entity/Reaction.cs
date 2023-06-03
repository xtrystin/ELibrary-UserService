namespace ELibrary_UserService.Domain.Entity;
public class Reaction
{
    public int? Id { get; private set; }    // null -> will be set by DB
    private bool _like;  

    public int BookId { get; set; }
    public string UserId { get; set; }

    public Book Book { get; set; } = null;
    public User User { get; set; } = null;

    protected Reaction()
    {
    }

    public Reaction(Book book, User user, bool like)
    {
        Book = book;
        BookId = book.Id;
        User = user;
        UserId = user.Id;

        _like = like;
    }

    public void Like() => _like = true;
    public void Dislike() => _like = false;
}
