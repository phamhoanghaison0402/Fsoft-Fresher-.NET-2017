using System;

namespace IPM.Common.CustomExceptions
{
    /// <summary>
    /// Exception happened during database action execution.
    /// </summary>
    public class DatabaseException : Exception
    {
        public DatabaseException()
        {
        }

        public DatabaseException(string message)
            : base(message)
        {
        }

        public DatabaseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
