namespace Movies.Client
{
    using Marvin.StreamExtensions;
    using Movies.Client.Models;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="MoviesClient" />
    /// </summary>
    public class MoviesClient
    {
        /// <summary>
        /// Defines the _client
        /// </summary>
        private HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoviesClient"/> class.
        /// </summary>
        /// <param name="client">The client<see cref="HttpClient"/></param>
        public MoviesClient(HttpClient client)
        {
            this._client = client;
            this._client.BaseAddress = new Uri("http://localhost:57863");
            this._client.Timeout = new TimeSpan(0, 0, 30);
            this._client.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// The GetMovies
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{IEnumerable{Movie}}"/></returns>
        public async Task<IEnumerable<Movie>> GetMovies(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (HttpResponseMessage response = await this._client.SendAsync(request,
              HttpCompletionOption.ResponseHeadersRead,
              cancellationToken))
            {
                System.IO.Stream stream = await response.Content.ReadAsStreamAsync();
                response.EnsureSuccessStatusCode();

                return stream.ReadAndDeserializeFromJson<List<Movie>>();
            }
        }
    }
}
