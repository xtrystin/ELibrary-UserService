using ELibrary_UserService.Domain.Exception;
using ELibrary_UserService.Domain.ValueObject;

namespace ELibrary_UserService.Domain.Entity
{
    public class User
    {
        public string Id { get; private set; }
        private string _firstName;
        private string _lastName;
        private bool _isAccountBlocked;
        private Description? _description;
        private decimal _amountToPay;

        private List<Book> _watchList = new();
        public IReadOnlyCollection<Book> WatchList => _watchList;

        List<Reaction> _reactions = new();
        public IReadOnlyCollection<Reaction> Reactions => _reactions;

        List<Review> _reviews = new();
        public IReadOnlyCollection<Review> Reviews => _reviews;

        public bool IsBlocked => _isAccountBlocked;
        public void Block() => _isAccountBlocked = true;
        public void UnBlock() => _isAccountBlocked = false;

        private readonly int MAX_AMOUNT_UNTIL_BLOCK = 200;

        protected User() { }

        public User(string id, string firstName, string lastName)
        {
            Id = id;
            _firstName = firstName;
            _lastName = lastName;
            _isAccountBlocked = false;
            _description = null;
            _amountToPay = 0;
        }

        public void AddToWatchList(Book book)
        {
            if (_watchList.Contains(book))
                throw new AlreadyExistsException("Book has been already added");
            _watchList.Add(book);
        }

        public void RemoveFromWatchList(Book book)
        {
            if (_watchList.Contains(book))
                throw new NoItemException("Book is not in the list");
            _watchList.Remove(book);
        }

        public void Modify(string firstname, string lastname, string? description)
        {
            _firstName = string.IsNullOrEmpty(firstname) ? _firstName : firstname;
            _lastName = string.IsNullOrEmpty(lastname) ? _lastName : lastname;
            _description = description is null ? _description : (description == "") ? null : new Description(description);
        }

        public void Pay(decimal amount)
        {
            if (_amountToPay - amount < 0)
                throw new System.Exception("Account amount to pay cannot be lower than 0");

            _amountToPay -= amount;
        }

        public void AddAmountToPay(decimal amount)
        {
            _amountToPay += amount;
            if (_amountToPay >= MAX_AMOUNT_UNTIL_BLOCK)
            {
                Block();
                throw new UserBlockedException();
            }
        }

        public void AddOrModifyReaction(Book book, bool like)
        {
            var reaction = _reactions.FirstOrDefault(x => x.Book == book);
            if (reaction is null)
            {
                reaction = new Reaction(book, this, like);
                _reactions.Add(reaction);
            }
            else
            {
                if (like is true)
                    reaction.Like();
                else
                    reaction.Dislike();
            }
        }

        public void RemoveReaction(Book book)
        {
            var reaction = _reactions.FirstOrDefault(x => x.Book == book);
            if (reaction is null)
                throw new NoItemException("Reaction for this user and book has not been found");
            _reactions.Remove(reaction);
        }

        public void AddOrModifyReview(Book book, string content)
        {
            var review = _reviews.FirstOrDefault(x => x.Book == book);
            if (review is null)
            {
                review = new Review(book, this, content);
                _reviews.Add(review);
            }
            else
            {
                review.ChangeContent(content);
            }
        }

        public void RemoveReview(Book book)
        {
            var review = _reviews.FirstOrDefault(x => x.Book == book);
            if (review is null)
                throw new NoItemException("Review for this user and book has not been found");
            _reviews.Remove(review);
        }
    }
}
