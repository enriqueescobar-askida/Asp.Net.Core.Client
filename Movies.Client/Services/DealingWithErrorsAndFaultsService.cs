namespace Movies.Client.Services
{
    using Marvin.StreamExtensions;
    using Movies.Client.Models;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DealingWithErrorsAndFaultsService" />
    /// </summary>
    public class DealingWithErrorsAndFaultsService : IIntegrationService
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
        /// Initializes a new instance of the <see cref="DealingWithErrorsAndFaultsService"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The httpClientFactory<see cref="IHttpClientFactory"/></param>
        public DealingWithErrorsAndFaultsService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// The Run
        /// await GetMovieAndDealWithInvalidResponses(_cancellationTokenSource.Token);
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Run() => await this.PostMovieAndHandleValdationErrors(this._cancellationTokenSource.Token);

        /// <summary>
        /// The GetMovieAndDealWithInvalidResponses
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetMovieAndDealWithInvalidResponses(CancellationToken cancellationToken)
        {
            HttpClient httpClient = this._httpClientFactory.CreateClient(@"MoviesClient");
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/movies/030a43b0-f9a5-405a-811c-bf342524b2be");
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

        /// <summary>
        /// The PostMovieAndHandleValdationErrors
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task"/></returns>
        private async Task PostMovieAndHandleValdationErrors(CancellationToken cancellationToken)
        {
            HttpClient httpClient = this._httpClientFactory.CreateClient(@"MoviesClient");
            MovieForCreation movieForCreation = new MovieForCreation
            {
                Title = "Pulp Fiction",
                Description = "Too short",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            string serializedMovieForCreation = JsonConvert.SerializeObject(movieForCreation);

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/movies"))
            {
                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                request.Content = new StringContent(serializedMovieForCreation);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (HttpResponseMessage response = await httpClient.SendAsync(request,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                        {
                            Stream errorStream = await response.Content.ReadAsStreamAsync();
                            object validationErrors = errorStream.ReadAndDeserializeFromJson();
                            Console.WriteLine(validationErrors);

                            return;
                        }
                        else
                            response.EnsureSuccessStatusCode();
                    }

                    Stream stream = await response.Content.ReadAsStreamAsync();
                    Movie movie = stream.ReadAndDeserializeFromJson<Movie>();
                }
            }
        }
    }
}
