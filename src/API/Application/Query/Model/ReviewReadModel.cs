namespace ELibrary_UserService.Application.Query.Model;

public class ReviewReadModel
{
    public int ReviewId { get; set; }
    public int BookId { get; set; }
    public string Content { get; set; }

    public ReviewReadModel(int id, int bookId, string content)
    {
        ReviewId = id;
        BookId = bookId;
        Content = content;
    }
}
