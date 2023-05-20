namespace ELibrary_UserService.Domain.Entity
{
    public class Book
    {
        public int Id { get; private set; }

        private readonly List<User> _users = new();
        public IReadOnlyCollection<User> Users => _users;

        protected Book() { }

        public Book(int id)
        {
            Id = id;
        }
    }
}
