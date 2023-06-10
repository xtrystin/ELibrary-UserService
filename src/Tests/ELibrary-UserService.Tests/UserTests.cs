

namespace ELibrary_UserService.Tests;

[TestFixture]
public class UserTests
{
    private User _user;

    [SetUp]
    public void Setup()
    {
        // Initialize a new User instance for each test
        _user = new User("1", "John", "Doe");
    }

    [Test]
    public void AddToWatchList_BookNotInWatchList_AddsBookToWatchList()
    {
        // Arrange
        var book = new Book(1);

        // Act
        _user.AddToWatchList(book);

        // Assert
        Assert.Contains(book, _user.WatchList.ToList());
    }

    [Test]
    public void AddToWatchList_BookAlreadyInWatchList_ThrowsAlreadyExistsException()
    {
        // Arrange
        var book = new Book(1);
        _user.AddToWatchList(book);

        // Act & Assert
        Assert.Throws<AlreadyExistsException>(() => _user.AddToWatchList(book));
    }

    [Test]
    public void RemoveFromWatchList_BookInWatchList_RemovesBookFromWatchList()
    {
        // Arrange
        var book = new Book(1);
        _user.AddToWatchList(book);

        // Act
        _user.RemoveFromWatchList(book);

        // Assert
        Assert.IsEmpty(_user.WatchList);
    }

    [Test]
    public void RemoveFromWatchList_BookNotInWatchList_ThrowsNoItemException()
    {
        // Arrange
        var book = new Book(1);

        // Act & Assert
        Assert.Throws<NoItemException>(() => _user.RemoveFromWatchList(book));
    }

    [Test]
    public void Modify_ValidArguments_ModifiesUserProperties()
    {
        // Arrange
        string firstName = "Jane";
        string lastName = "Smith";
        string description = "New description";

        // Act
        _user.Modify(firstName, lastName, description);

        // Assert
        Assert.AreEqual(firstName, _user.GetType().GetField("_firstName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_user));
        Assert.AreEqual(lastName, _user.GetType().GetField("_lastName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_user));
        Assert.AreEqual(description, (_user.GetType().GetField("_description", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_user) as Description)?.Value);
    }

    [Test]
    public void Modify_NullArguments_DoesNotModifyUserProperties()
    {
        // Arrange
        string firstName = "Jane";
        string lastName = "Smith";
        string description = "New description";
        _user.Modify(firstName, lastName, description);

        // Act
        _user.Modify(null, null, null);

        // Assert
        Assert.AreEqual(firstName, _user.GetType().GetField("_firstName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_user));
        Assert.AreEqual(lastName, _user.GetType().GetField("_lastName", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_user));
        Assert.AreEqual(description, (_user.GetType().GetField("_description", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_user) as Description)?.Value);
    }

    [Test]
    public void Pay_AmountToPayLessThanAmount_ThrowsException()
    {
        // Arrange
        decimal amount = 100;
        _user.AddAmountToPay(amount);

        // Act & Assert
        Assert.Throws<Exception>(() => _user.Pay(amount + 1));
    }

    [Test]
    public void Pay_AmountToPayEqualsAmount_NoExceptionThrown()
    {
        // Arrange
        decimal amount = 100;
        _user.AddAmountToPay(amount);

        // Act & Assert
        Assert.DoesNotThrow(() => _user.Pay(amount));
    }

    [Test]
    public void AddAmountToPay_AmountExceedsThreshold_UserIsBlocked()
    {
        // Arrange
        decimal amount = 300;

        // Act & Assert
        Assert.Throws<UserBlockedException>(() => _user.AddAmountToPay(amount));
        Assert.IsTrue(_user.IsBlocked);
    }

    [Test]
    public void AddOrModifyReaction_ReactionDoesNotExist_AddsReaction()
    {
        // Arrange
        var book = new Book(1);

        // Act
        _user.AddOrModifyReaction(book, true);

        // Assert
        Assert.AreEqual(1, _user.Reactions.Count);
    }

    [Test]
    public void AddOrModifyReaction_ReactionExistsWithDifferentLikeValue_ModifiesReaction()
    {
        // Arrange
        var book = new Book(1);
        var reactionLikeField = typeof(Reaction).GetField("_like", BindingFlags.NonPublic | BindingFlags.Instance);
        _user.AddOrModifyReaction(book, true);

        // Act
        _user.AddOrModifyReaction(book, false);

        // Assert
        Assert.AreEqual(1, _user.Reactions.Count);
        var reaction = _user.Reactions.FirstOrDefault(r => r.Book == book);
        Assert.IsFalse((bool)reactionLikeField.GetValue(reaction));
    }

    [Test]
    public void RemoveReaction_ReactionExists_RemovesReaction()
    {
        // Arrange
        var book = new Book(1);
        _user.AddOrModifyReaction(book, true);

        // Act
        _user.RemoveReaction(book);

        // Assert
        Assert.AreEqual(0, _user.Reactions.Count);
    }

    [Test]
    public void RemoveReaction_ReactionDoesNotExist_ThrowsNoItemException()
    {
        // Arrange
        var book = new Book(1);

        // Act & Assert
        Assert.Throws<NoItemException>(() => _user.RemoveReaction(book));
    }

    [Test]
    public void AddOrModifyReview_ReviewDoesNotExist_AddsReview()
    {
        // Arrange
        var book = new Book(1);
        string content = "Great book!";

        // Act
        _user.AddOrModifyReview(book, content);

        // Assert
        Assert.AreEqual(1, _user.Reviews.Count);
    }

    [Test]
    public void AddOrModifyReview_ReviewExists_ModifiesReview()
    {
        // Arrange
        var book = new Book(1);
        string content = "Great book!";
        _user.AddOrModifyReview(book, content);

        // Act
        string newContent = "Awesome book!";
        _user.AddOrModifyReview(book, newContent);

        // Assert
        var review = _user.Reviews.FirstOrDefault(r => r.Book == book);
        Assert.AreEqual(newContent, review.GetType().GetField("_content", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(review));
        
    }

    [Test]
    public void RemoveReview_ReviewExists_RemovesReview()
    {
        // Arrange
        var book = new Book(1);
        string content = "Great book!";
        _user.AddOrModifyReview(book, content);

        // Act
        _user.RemoveReview(book);

        // Assert
        Assert.AreEqual(0, _user.Reviews.Count);
    }

    [Test]
    public void RemoveReview_ReviewDoesNotExist_ThrowsNoItemException()
    {
        // Arrange
        var book = new Book(1);

        // Act & Assert
        Assert.Throws<NoItemException>(() => _user.RemoveReview(book));
    }
}