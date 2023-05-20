using ELibrary_UserService.Domain.Exception;
using ELibrary_UserService.Domain.ValueObject;

namespace ELibrary_UserService.Domain.Entity
{
    public class User
    {
        public Guid Id { get; private set; }
        private string _firstName;
        private string _lastName;
        private bool _isAccountBlocked;
        private Description? _description;
        private decimal _amountToPay;

        private List<Book> _watchList = new();
        public IReadOnlyCollection<Book> WatchList => _watchList;

        public bool IsBlocked => _isAccountBlocked;
        public void Block() => _isAccountBlocked = true;
        public void UnBlock() => _isAccountBlocked = false;

        protected User() { }

        public User(Guid id, string firstName, string lastName)
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

        public void ModifyDescription(Description description)
            => _description = description;

        public void Pay(decimal amount)
        {
            if (_amountToPay - amount < 0)
                throw new System.Exception("Account amount to pay cannot be lower than 0");
        }

        public void AddAmountToPay(decimal amount)
            => _amountToPay += amount;
    }
}
