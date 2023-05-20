namespace ELibrary_UserService.Domain.Exception
{
    public class NoItemException : System.Exception
    {
        public NoItemException(string message) : base(message)
        {
        }
    }
}
