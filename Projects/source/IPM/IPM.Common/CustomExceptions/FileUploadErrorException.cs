using System;

namespace IPM.Common.CustomExceptions
{
    /// <summary>
    /// Exception throwns during file uploading execution.
    /// </summary>
    public class FileUploadErrorException : Exception
    {
        public FileUploadErrorException()
        {
        }

        public FileUploadErrorException(string message)
            : base(message)
        {
        }

        public FileUploadErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
