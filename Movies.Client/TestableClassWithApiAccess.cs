namespace Movies.Client
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
    /// Defines the <see cref="TestableClassWithApiAccess" />
    /// </summary>
    public class TestableClassWithApiAccess
    {
        /// <summary>
        /// Defines the _httpClient
        /// </summary>
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestableClassWithApiAccess"/> class.
        /// </summary>
        /// <param name="httpClient">The httpClient<see cref="HttpClient"/></param>
        public TestableClassWithApiAccess(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        /// <summary>
        /// The GetMovie
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken<see cref="CancellationToken"/></param>
        /// <returns>The <see cref="Task{Movie}"/></returns>
        public async Task<Movie> GetMovie(CancellationToken cancellationToken)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,
                "api/movies/030a43b0-f9a5-405a-811c-bf342524b2be");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (HttpResponseMessage response = await this._httpClient.SendAsync(request,
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

                        return null;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        // trigger a login flow
                        throw new UnauthorizedApiAccessException();

                    response.EnsureSuccessStatusCode();
                }

                Stream stream = await response.Content.ReadAsStreamAsync();

                return stream.ReadAndDeserializeFromJson<Movie>();
            }
        }
    }
}
