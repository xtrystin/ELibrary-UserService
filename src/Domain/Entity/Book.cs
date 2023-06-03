namespace ELibrary_UserService.Domain.Entity
{
    public class Book
    {
        public int Id { get; private set; }

        private readonly List<User> _users = new();
        private readonly List<Reaction> _reactions = new();
        private readonly List<Review> _reviews = new();
        public IReadOnlyCollection<User> Users => _users;
        public IReadOnlyCollection<Reaction> Reactions => _reactions;
        public IReadOnlyCollection<Review> Reviews => _reviews;

        protected Book() { }

        public Book(int id)
        {
            Id = id;
        }
    }
}
