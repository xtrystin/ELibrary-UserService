namespace ELibrary_UserService.Application.Query.Model;

public class ReactionReadModel
{
    public int ReactionId { get; set; }
    public int BookId { get; set; }
    public bool Like { get; set; }

    public ReactionReadModel(int id, int bookId, bool like)
    {
        ReactionId = id;
        BookId = bookId;
        Like = like;
    }
}
