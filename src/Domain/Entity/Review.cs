namespace ELibrary_UserService.Domain.Entity;
public class Review
{
    public int? Id { get; private set; }    // null -> will be set by DB
    private string _content;
    public int BookId { get; set; }
    public string UserId { get; set; }

    public Book Book { get; set; } = null;
    public User User { get; set; } = null;

    protected Review()
    {
    }

    public Review(Book book, User user, string content)
    {
        Book = book;
        BookId = book.Id;
        User = user;
        UserId = user.Id;

        _content = content;
    }

    public void ChangeContent(string newcontent) => _content = newcontent;
}
