using System;

namespace GamesManager.Infrastructure.Services
{
    public class InvalidFormException : Exception
    {
        public InvalidFormException(string message) : base(message)
        {
        }
    }

    public class InvalidInnerStateException : Exception
    {
        public InvalidInnerStateException(string message) : base(message)
        {
        }
    }
}
