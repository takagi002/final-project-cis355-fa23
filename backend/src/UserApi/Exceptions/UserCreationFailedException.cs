using System;

namespace UserApi.Exceptions
{
    public class UserCreationFailedException : Exception
    {
        public UserCreationFailedException(string message) : base(message)
        {
        }

        public UserCreationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
