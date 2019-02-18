namespace Movies.Client.Services
{
    using Movies.Client.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    /// <summary>
    /// Defines the <see cref="CRUDService" />
    /// </summary>
    public class CRUDService : IIntegrationService
    {
        /// <summary>
        /// Defines the _httpClient
        /// </summary>
        private static HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="CRUDService"/> class.
        /// </summary>
        public CRUDService()
        {
            // set up HttpClient instance
            _httpClient.BaseAddress = new Uri("http://localhost:57863");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// The Run
        /// await GetResource();
        /// await GetResourceThroughHttpRequestMessage();
        /// await CreateResource();
        /// await UpdateResource();
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Run() => await this.DeleteResource();

        /// <summary>
        /// The GetResource
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task GetResource()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/movies");
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            List<Movie> movies = new List<Movie>();

            if (response.Content.Headers.ContentType.MediaType == "application/json")
                movies = JsonConvert.DeserializeObject<List<Movie>>(content);
            else if (response.Content.Headers.ContentType.MediaType == "application/xml")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Movie>));
                movies = (List<Movie>)serializer.Deserialize(new StringReader(content));
            }
        }

        /// <summary>
        /// The GetResourceThroughHttpRequestMessage
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task GetResourceThroughHttpRequestMessage()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            List<Movie> movies = JsonConvert.DeserializeObject<List<Movie>>(content);
        }

        /// <summary>
        /// The CreateResource
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task CreateResource()
        {
            MovieForCreation movieToCreate = new MovieForCreation()
            {
                Title = "Reservoir Dogs",
                Description = "After a simple jewelry heist goes terribly wrong, the " +
                 "surviving criminals begin to suspect that one of them is a police informant.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            string serializedMovieToCreate = JsonConvert.SerializeObject(movieToCreate);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/movies");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(serializedMovieToCreate);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Movie createdMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        /// <summary>
        /// The UpdateResource
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task UpdateResource()
        {
            MovieForUpdate movieToUpdate = new MovieForUpdate()
            {
                Title = "Pulp Fiction",
                Description = "The movie with Zed.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            string serializedMovieToUpdate = JsonConvert.SerializeObject(movieToUpdate);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put,
                "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(serializedMovieToUpdate);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Movie updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        /// <summary>
        /// The DeleteResource
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task DeleteResource()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete,
                "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// The PostResourceShortcut
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task PostResourceShortcut()
        {
            MovieForCreation movieToCreate = new MovieForCreation
            {
                Title = "Reservoir Dogs",
                Description = "After a simple jewelry heist goes terribly wrong, the " +
                "surviving criminals begin to suspect that one of them is a police informant.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            HttpResponseMessage response = await _httpClient.PostAsync(
                "api/movies",
                new StringContent(
                    JsonConvert.SerializeObject(movieToCreate),
                    Encoding.UTF8,
                    "application/json"));

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Movie createdMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        /// <summary>
        /// The PutResourceShortcut
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task PutResourceShortcut()
        {
            MovieForUpdate movieToUpdate = new MovieForUpdate
            {
                Title = "Pulp Fiction",
                Description = "The movie with Zed.",
                DirectorId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"),
                ReleaseDate = new DateTimeOffset(new DateTime(1992, 9, 2)),
                Genre = "Crime, Drama"
            };

            HttpResponseMessage response = await _httpClient.PutAsync(
               "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b",
               new StringContent(
                   JsonConvert.SerializeObject(movieToUpdate),
                   Encoding.UTF8,
                   "application/json"));

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Movie updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        /// <summary>
        /// The DeleteResourceShortcut
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task DeleteResourceShortcut()
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(
                "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
        }
    }
}
