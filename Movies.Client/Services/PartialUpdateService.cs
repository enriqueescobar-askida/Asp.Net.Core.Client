namespace Movies.Client.Services
{
    using Microsoft.AspNetCore.JsonPatch;
    using Movies.Client.Models;
    using Newtonsoft.Json;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="PartialUpdateService" />
    /// </summary>
    public class PartialUpdateService : IIntegrationService
    {
        /// <summary>
        /// Defines the _httpClient
        /// </summary>
        private static HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="PartialUpdateService"/> class.
        /// </summary>
        public PartialUpdateService()
        {
            // set up HttpClient instance
            _httpClient.BaseAddress = new Uri("http://localhost:57863");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// The Run
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Run()
        {
            await this.PatchResource();
        }

        /// <summary>
        /// The PatchResource
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task PatchResource()
        {
            JsonPatchDocument<MovieForUpdate> patchDoc = new JsonPatchDocument<MovieForUpdate>();
            patchDoc.Replace(m => m.Title, "Updated Title");
            patchDoc.Remove(m => m.Description);

            string serializedChangeSet = JsonConvert.SerializeObject(patchDoc);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Patch,
                "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(serializedChangeSet);
            request.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/json-patch+json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Movie updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
        }

        /// <summary>
        /// The PatchResourceShortcut
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task PatchResourceShortcut()
        {
            JsonPatchDocument<MovieForUpdate> patchDoc = new JsonPatchDocument<MovieForUpdate>();
            patchDoc.Replace(m => m.Title, "Updated Title");
            patchDoc.Remove(m => m.Description);

            HttpResponseMessage response = await _httpClient.PatchAsync(
               "api/movies/5b1c2b4d-48c7-402a-80c3-cc796ad49c6b",
               new StringContent(
                   JsonConvert.SerializeObject(patchDoc),
                  Encoding.UTF8,
                   "application/json-patch+json"));

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Movie updatedMovie = JsonConvert.DeserializeObject<Movie>(content);
        }
    }
}
