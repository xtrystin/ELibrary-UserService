using ELibrary_UserService.Domain.Exception;

namespace ELibrary_UserService.Domain.ValueObject
{
    public record Description : ValueObject<string>
    {
        public static readonly int MaxLength = 10000;

        public Description(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new NoItemException("Description cannot be empty");
            if (value.Length > MaxLength)
                throw new TooLongStringException($"Description cannot be longer than {MaxLength} characters");

            _value = value;
        }
    }
}
