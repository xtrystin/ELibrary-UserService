namespace ELibrary_UserService.Application.Command.Exception
{
    public class AlreadyExistsException : System.Exception
    {
        public AlreadyExistsException(string message) : base(message) { }
    }
}
