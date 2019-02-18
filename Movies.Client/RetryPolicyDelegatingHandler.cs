namespace Movies.Client
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="RetryPolicyDelegatingHandler" />
    /// </summary>
    public class RetryPolicyDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        /// Defines the _maximumAmountOfRetries
        /// </summary>
        private readonly int _maximumAmountOfRetries = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicyDelegatingHandler"/> class.
        /// </summary>
        /// <param name="maximumAmountOfRetries">The maximumAmountOfRetries<see cref="int"/></param>
        public RetryPolicyDelegatingHandler(int maximumAmountOfRetries)
            : base()
        {
            this._maximumAmountOfRetries = maximumAmountOfRetries;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryPolicyDelegatingHandler"/> class.
        /// </summary>
        /// <param name="innerHandler">The innerHandler<see cref="HttpMessageHandler"/></param>
        /// <param name="maximumAmountOfRetries">The maximumAmountOfRetries<see cref="int"/></param>
        public RetryPolicyDelegatingHandler(HttpMessageHandler innerHandler, int maximumAmountOfRetries)
        : base(innerHandler)
        {
            this._maximumAmountOfRetries = maximumAmountOfRetries;
        }

        /// <summary>
        /// The SendAsync
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequestMessage"/></param>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{HttpResponseMessage}"/></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            for (int i = 0; i < this._maximumAmountOfRetries; i++)
            {
                response = await base.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                    return response;
            }

            return response;
        }
    }
}
