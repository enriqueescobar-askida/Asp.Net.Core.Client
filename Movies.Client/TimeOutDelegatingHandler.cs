namespace Movies.Client
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="TimeOutDelegatingHandler" />
    /// </summary>
    public class TimeOutDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        /// Defines the _timeOut
        /// </summary>
        private readonly TimeSpan _timeOut = TimeSpan.FromSeconds(100);

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOutDelegatingHandler"/> class.
        /// </summary>
        /// <param name="timeOut">The timeOut<see cref="TimeSpan"/></param>
        public TimeOutDelegatingHandler(TimeSpan timeOut)
            : base()
        {
            this._timeOut = timeOut;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeOutDelegatingHandler"/> class.
        /// </summary>
        /// <param name="innerHandler">The innerHandler<see cref="HttpMessageHandler"/></param>
        /// <param name="timeOut">The timeOut<see cref="TimeSpan"/></param>
        public TimeOutDelegatingHandler(HttpMessageHandler innerHandler,
           TimeSpan timeOut)
        : base(innerHandler)
        {
            this._timeOut = timeOut;
        }

        /// <summary>
        /// The SendAsync
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequestMessage"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/></returns>
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (CancellationTokenSource linkedCancellationTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                linkedCancellationTokenSource.CancelAfter(this._timeOut);
                try
                {
                    return await base.SendAsync(request, linkedCancellationTokenSource.Token);
                }
                catch (OperationCanceledException ex)
                {
                    if (!cancellationToken.IsCancellationRequested)
                        throw new TimeoutException("The request timed out.", ex);

                    throw;
                }
            }
        }
    }
}
