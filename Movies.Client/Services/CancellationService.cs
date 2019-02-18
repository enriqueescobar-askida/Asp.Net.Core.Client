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
    /// Defines the <see cref="CancellationService" />
    /// </summary>
    public class CancellationService : IIntegrationService
    {
        /// <summary>
        /// Defines the _httpClient
        /// </summary>
        private static HttpClient _httpClient = new HttpClient(
          new HttpClientHandler()
          {
              AutomaticDecompression = System.Net.DecompressionMethods.GZip
          });

        /// <summary>
        /// Defines the _cancellationTokenSource
        /// </summary>
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationService"/> class.
        /// </summary>
        public CancellationService()
        {
            // set up HttpClient instance
            _httpClient.BaseAddress = new Uri("http://localhost:57863");
            _httpClient.Timeout = new TimeSpan(0, 0, 5);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// The Run
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Run()
        {
            //_cancellationTokenSource.CancelAfter(2000);
            //await GetTrailerAndCancel(_cancellationTokenSource.Token);
            await this.GetTrailerAndHandleTimeout();
        }

        /// <summary>
        /// The GetTrailerAndCancel
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetTrailerAndCancel(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/trailers/{Guid.NewGuid()}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            //var cancellationTokenSource = new CancellationTokenSource();
            //cancellationTokenSource.CancelAfter(2000);
            try
            {
                using (HttpResponseMessage response = await _httpClient.SendAsync(request,
                   HttpCompletionOption.ResponseHeadersRead,
                   cancellationToken))
                {
                    Stream stream = await response.Content.ReadAsStreamAsync();

                    response.EnsureSuccessStatusCode();
                    Trailer trailer = stream.ReadAndDeserializeFromJson<Trailer>();
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"An operation was cancelled with message {ocException.Message}.");
                // additional cleanup, ...
            }
        }

        /// <summary>
        /// The GetTrailerAndHandleTimeout
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetTrailerAndHandleTimeout()
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/trailers/{Guid.NewGuid()}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            try
            {
                using (HttpResponseMessage response = await _httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    Stream stream = await response.Content.ReadAsStreamAsync();

                    response.EnsureSuccessStatusCode();
                    Trailer trailer = stream.ReadAndDeserializeFromJson<Trailer>();
                }
            }
            catch (OperationCanceledException ocException)
            {
                Console.WriteLine($"An operation was cancelled with message {ocException.Message}.");
                // additional cleanup, ...
            }
        }
    }
}
