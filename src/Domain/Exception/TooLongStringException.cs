namespace ELibrary_UserService.Domain.Exception
{
    internal class TooLongStringException : System.Exception
    {
        public TooLongStringException(string message) : base(message)
        {
        }
    }
}
