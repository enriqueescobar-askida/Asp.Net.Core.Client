namespace Movies.Client
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines the <see cref="UnauthorizedApiAccessException" />
    /// </summary>
    [Serializable]
    public class UnauthorizedApiAccessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedApiAccessException"/> class.
        /// </summary>
        public UnauthorizedApiAccessException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedApiAccessException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        public UnauthorizedApiAccessException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedApiAccessException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        /// <param name="innerException">The innerException<see cref="Exception"/></param>
        public UnauthorizedApiAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedApiAccessException"/> class.
        /// </summary>
        /// <param name="info">The info<see cref="SerializationInfo"/></param>
        /// <param name="context">The context<see cref="StreamingContext"/></param>
        protected UnauthorizedApiAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
