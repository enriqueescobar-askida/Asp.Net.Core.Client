namespace Movies.Client.Services
{
    using Marvin.StreamExtensions;
    using Movies.Client.Models;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="HttpHandlersService" />
    /// </summary>
    public class HttpHandlersService : IIntegrationService
    {
        /// <summary>
        /// Defines the _httpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Defines the _cancellationTokenSource
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Defines the _notSoNicelyInstantiatedHttpClient
        /// </summary>
        private static HttpClient _notSoNicelyInstantiatedHttpClient =
           new HttpClient(
               new RetryPolicyDelegatingHandler(
                   new HttpClientHandler()
                   { AutomaticDecompression = System.Net.DecompressionMethods.GZip },
                   2));

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandlersService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The httpClientFactory<see cref="IHttpClientFactory"/></param>
        public HttpHandlersService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// The Run
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Run()
        {
            await this.GetMoviesWithRetryPolicy(this._cancellationTokenSource.Token);
        }

        /// <summary>
        /// The GetMoviesWithRetryPolicy
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        public async Task GetMoviesWithRetryPolicy(CancellationToken cancellationToken)
        {
            HttpClient httpClient = this._httpClientFactory.CreateClient("MoviesClient");
            // var request = new HttpRequestMessage(
            //    HttpMethod.Get,
            //    "api/movies/030a43b0-f9a5-405a-811c-bf342524b2be");
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (HttpResponseMessage response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    // inspect the status code
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        // show this to the user
                        Console.WriteLine("The requested movie cannot be found.");

                        return;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        // trigger a login flow
                        return;
                    response.EnsureSuccessStatusCode();
                }

                Stream stream = await response.Content.ReadAsStreamAsync();
                Movie movie = stream.ReadAndDeserializeFromJson<Movie>();
            }
        }
    }
}
