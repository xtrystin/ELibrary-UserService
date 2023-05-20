namespace ELibrary_UserService.Domain.Exception
{
    public class AlreadyExistsException : System.Exception
    {
        public AlreadyExistsException(string message) : base(message)
        {
        }
    }
}
