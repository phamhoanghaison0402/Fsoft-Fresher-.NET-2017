using System;

namespace IPM.Common.CustomExceptions
{
    /// <summary>
    /// Exception happened when call save changes on data context.
    /// </summary>
    public class CommitException : Exception
    {
        public CommitException()
        {
        }

        public CommitException(string message)
            : base(message)
        {
        }

        public CommitException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
