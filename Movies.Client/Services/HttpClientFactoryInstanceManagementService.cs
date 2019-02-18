namespace Movies.Client.Services
{
    using Marvin.StreamExtensions;
    using Movies.Client.Models;
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="HttpClientFactoryInstanceManagementService" />
    /// </summary>
    public class HttpClientFactoryInstanceManagementService : IIntegrationService
    {
        /// <summary>
        /// Defines the _cancellationTokenSource
        /// </summary>
        private readonly CancellationTokenSource _cancellationTokenSource =
            new CancellationTokenSource();

        /// <summary>
        /// Defines the _httpClientFactory
        /// </summary>
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Defines the _moviesClient
        /// </summary>
        private readonly MoviesClient _moviesClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientFactoryInstanceManagementService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The httpClientFactory<see cref="IHttpClientFactory"/></param>
        /// <param name="moviesClient">The moviesClient<see cref="MoviesClient"/></param>
        public HttpClientFactoryInstanceManagementService(IHttpClientFactory httpClientFactory,
            MoviesClient moviesClient)
        {
            this._httpClientFactory = httpClientFactory;
            this._moviesClient = moviesClient;
        }

        /// <summary>
        /// The Run
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Run()
        {
            // await TestDisposeHttpClient(_cancellationTokenSource.Token);
            // await TestReuseHttpClient(_cancellationTokenSource.Token);
            // await GetMoviesWithHttpClientFromFactory(_cancellationTokenSource.Token);
            // await GetMoviesWithNamedHttpClientFromFactory(_cancellationTokenSource.Token);
            // await GetMoviesWithTypedHttpClientFromFactory(_cancellationTokenSource.Token);
            await this.GetMoviesViaMoviesClient(this._cancellationTokenSource.Token);
        }

        /// <summary>
        /// The TestDisposeHttpClient
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task TestDisposeHttpClient(CancellationToken cancellationToken)
        {
            for (int i = 0; i < 10; i++)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpRequestMessage request = new HttpRequestMessage(
                        HttpMethod.Get,
                        "https://www.google.com");

                    using (HttpResponseMessage response = await httpClient.SendAsync(request,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken))
                    {
                        Stream stream = await response.Content.ReadAsStreamAsync();
                        response.EnsureSuccessStatusCode();

                        Console.WriteLine($"Request completed with status code" + $" {response.StatusCode}");
                    }
                }
            }
        }

        /// <summary>
        /// The TestReuseHttpClient
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task TestReuseHttpClient(CancellationToken cancellationToken)
        {
            HttpClient httpClient = new HttpClient();

            for (int i = 0; i < 10; i++)
            {
                HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                "https://www.google.com");
                using (HttpResponseMessage response = await httpClient.SendAsync(request,
                    HttpCompletionOption.ResponseHeadersRead,
                    cancellationToken))
                {
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    response.EnsureSuccessStatusCode();

                    Console.WriteLine($"Request completed with status code {response.StatusCode}");
                }
            }
        }

        /// <summary>
        /// The GetMoviesWithHttpClientFromFactory
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetMoviesWithHttpClientFromFactory(
            CancellationToken cancellationToken)
        {
            HttpClient httpClient = this._httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                "http://localhost:57863/api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken))
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
                List<Movie> movies = stream.ReadAndDeserializeFromJson<List<Movie>>();
            }
        }

        /// <summary>
        /// The GetMoviesWithNamedHttpClientFromFactory
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetMoviesWithNamedHttpClientFromFactory(CancellationToken cancellationToken)
        {
            HttpClient httpClient = this._httpClientFactory.CreateClient("MoviesClient");
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (HttpResponseMessage response = await httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken))
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();
                List<Movie> movies = stream.ReadAndDeserializeFromJson<List<Movie>>();
            }
        }

        //private async Task GetMoviesWithTypedHttpClientFromFactory(
        //    CancellationToken cancellationToken)
        //{
        //    var request = new HttpRequestMessage(
        //        HttpMethod.Get,
        //        "api/movies");
        //    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

        //    using (var response = await _moviesClient.Client.SendAsync(request,
        //        HttpCompletionOption.ResponseHeadersRead,
        //        cancellationToken))
        //    {
        //        var stream = await response.Content.ReadAsStreamAsync();
        //        response.EnsureSuccessStatusCode();
        //        var movies = stream.ReadAndDeserializeFromJson<List<Movie>>();
        //    }
        //}
        /// <summary>
        /// The GetMoviesViaMoviesClient
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetMoviesViaMoviesClient(CancellationToken cancellationToken)
        {
            IEnumerable<Movie> movies = await this._moviesClient.GetMovies(cancellationToken);
        }
    }
}
