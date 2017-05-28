using System;

namespace IPM.Common.CustomExceptions
{
    /// <summary>
    /// Exception thrown when entity not found.
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
