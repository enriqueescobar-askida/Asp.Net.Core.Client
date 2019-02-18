namespace Movies.Client.Services
{
    using Marvin.StreamExtensions;
    using Movies.Client.Models;
    using Newtonsoft.Json;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="StreamService" />
    /// </summary>
    public class StreamService : IIntegrationService
    {
        //private static HttpClient _httpClient = new HttpClient();
        /// <summary>
        /// Defines the _httpClient
        /// </summary>
        private static HttpClient _httpClient = new HttpClient(
            new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
            });

        /// <summary>
        /// Initializes a new instance of the <see cref="StreamService"/> class.
        /// </summary>
        public StreamService()
        {
            // set up HttpClient instance
            _httpClient.BaseAddress = new Uri("http://localhost:57863");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// The Run
        /// await GetPosterWithStream();
        /// await GetPosterWithStreamAndCompletionMode();
        /// await TestGetPosterWithoutStream();
        /// await TestGetPosterWithStream();
        /// await TestGetPosterWithStreamAndCompletionMode();
        /// await PostPosterWithStream();
        /// await PostAndReadPosterWithStreams();
        /// await TestPostPosterWithoutStream();
        /// await TestPostPosterWithStream();
        /// await TestPostAndReadPosterWithStreams();
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task Run()
        {
            await this.GetPosterWithGZipCompression();
        }

        /// <summary>
        /// The GetPosterWithStream
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetPosterWithStream()
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters/{Guid.NewGuid()}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                Poster poster = stream.ReadAndDeserializeFromJson<Poster>();

                //using (var streamReader = new StreamReader(stream))
                //{
                //    using (var jsonTextReader = new JsonTextReader(streamReader))
                //    {
                //        var jsonSerializer = new JsonSerializer();
                //        var poster = jsonSerializer.Deserialize<Poster>(jsonTextReader);

                //        // do something with the poster
                //    }
                //}
            }
        }

        /// <summary>
        /// The GetPosterWithStreamAndCompletionMode
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetPosterWithStreamAndCompletionMode()
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters/{Guid.NewGuid()}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (HttpResponseMessage response = await _httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                Poster poster = stream.ReadAndDeserializeFromJson<Poster>();

                //using (var streamReader = new StreamReader(stream))
                //{
                //    using (var jsonTextReader = new JsonTextReader(streamReader))
                //    {
                //        var jsonSerializer = new JsonSerializer();
                //        var poster = jsonSerializer.Deserialize<Poster>(jsonTextReader);

                //        // do something with the poster
                //    }
                //}
            }
        }

        /// <summary>
        /// The GetPosterWithoutStream
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetPosterWithoutStream()
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters/{Guid.NewGuid()}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Poster posters = JsonConvert.DeserializeObject<Poster>(content);
        }

        /// <summary>
        /// The PostPosterWithStream
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task PostPosterWithStream()
        {
            // generate a movie poster of 500KB
            Random random = new Random();
            byte[] generatedBytes = new byte[524288];
            random.NextBytes(generatedBytes);

            PosterForCreation posterForCreation = new PosterForCreation()
            {
                Name = "A new poster for The Big Lebowski",
                Bytes = generatedBytes
            };

            MemoryStream memoryContentStream = new MemoryStream();
            memoryContentStream.SerializeToJsonAndWrite(posterForCreation,
                new UTF8Encoding(), 1024, true);

            //using (var streamWriter = new StreamWriter(memoryContentStream,
            //    new UTF8Encoding(), 1024, true))
            //{
            //    using (var jsonTextWriter = new JsonTextWriter(streamWriter))
            //    {
            //        var jsonSerializer = new JsonSerializer();
            //        jsonSerializer.Serialize(jsonTextWriter, posterForCreation);
            //        jsonTextWriter.Flush();
            //    }
            //}

            memoryContentStream.Seek(0, SeekOrigin.Begin);
            using (HttpRequestMessage request = new HttpRequestMessage(
              HttpMethod.Post,
              $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters"))
            {
                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                using (StreamContent streamContent = new StreamContent(memoryContentStream))
                {
                    request.Content = streamContent;
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    string createdContent = await response.Content.ReadAsStringAsync();
                    Poster createdPoster = JsonConvert.DeserializeObject<Poster>(createdContent);
                }
            }
        }

        /// <summary>
        /// The PostAndReadPosterWithStreams
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task PostAndReadPosterWithStreams()
        {
            // generate a movie poster of 500KB
            Random random = new Random();
            byte[] generatedBytes = new byte[524288];
            random.NextBytes(generatedBytes);

            PosterForCreation posterForCreation = new PosterForCreation()
            {
                Name = "A new poster for The Big Lebowski",
                Bytes = generatedBytes
            };

            MemoryStream memoryContentStream = new MemoryStream();
            memoryContentStream.SerializeToJsonAndWrite(posterForCreation, new UTF8Encoding(), 1024, true);

            memoryContentStream.Seek(0, SeekOrigin.Begin);
            using (HttpRequestMessage request = new HttpRequestMessage(
              HttpMethod.Post,
              $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters"))
            {
                request.Headers.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                using (StreamContent streamContent = new StreamContent(memoryContentStream))
                {
                    request.Content = streamContent;
                    request.Content.Headers.ContentType =
                      new MediaTypeHeaderValue("application/json");

                    using (HttpResponseMessage response = await _httpClient
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        response.EnsureSuccessStatusCode();
                        Stream stream = await response.Content.ReadAsStreamAsync();
                        Poster poster = stream.ReadAndDeserializeFromJson<Poster>();
                    }
                }
            }
        }

        /// <summary>
        /// The PostPosterWithoutStream
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task PostPosterWithoutStream()
        {
            // generate a movie poster of 500KB
            Random random = new Random();
            byte[] generatedBytes = new byte[524288];
            random.NextBytes(generatedBytes);

            PosterForCreation posterForCreation = new PosterForCreation()
            {
                Name = "A new poster for The Big Lebowski",
                Bytes = generatedBytes
            };

            string serializedPosterForCreation = JsonConvert.SerializeObject(posterForCreation);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
                "api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Content = new StringContent(serializedPosterForCreation);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            Poster createdMovie = JsonConvert.DeserializeObject<Poster>(content);
        }

        /// <summary>
        /// The GetPosterWithGZipCompression
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task GetPosterWithGZipCompression()
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/movies/d8663e5e-7494-4f81-8739-6e0de1bea7ee/posters/{Guid.NewGuid()}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (HttpResponseMessage response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                Stream stream = await response.Content.ReadAsStreamAsync();
                Poster poster = stream.ReadAndDeserializeFromJson<Poster>();
            }
        }

        /// <summary>
        /// The TestPostPosterWithoutStream
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task TestPostPosterWithoutStream()
        {
            // warmup
            await this.PostPosterWithoutStream();

            // start stopwatch
            Stopwatch stopWatch = Stopwatch.StartNew();

            // run requests
            for (int i = 0; i < 200; i++)
                await this.PostPosterWithoutStream();

            // stop stopwatch
            stopWatch.Stop();
            Console.WriteLine($"Elapsed milliseconds without stream: " +
                $"{stopWatch.ElapsedMilliseconds}, " +
                $"averaging {stopWatch.ElapsedMilliseconds / 200} milliseconds/request");
        }

        /// <summary>
        /// The TestPostPosterWithStream
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task TestPostPosterWithStream()
        {
            // warmup
            await this.PostPosterWithStream();

            // start stopwatch
            Stopwatch stopWatch = Stopwatch.StartNew();

            // run requests
            for (int i = 0; i < 200; i++)
                await this.PostPosterWithStream();

            // stop stopwatch
            stopWatch.Stop();
            Console.WriteLine($"Elapsed milliseconds with stream: " +
                $"{stopWatch.ElapsedMilliseconds}, " +
                $"averaging {stopWatch.ElapsedMilliseconds / 200} milliseconds/request");
        }

        /// <summary>
        /// The TestPostAndReadPosterWithStreams
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task TestPostAndReadPosterWithStreams()
        {
            // warmup
            await this.PostAndReadPosterWithStreams();

            // start stopwatch
            Stopwatch stopWatch = Stopwatch.StartNew();

            // run requests
            for (int i = 0; i < 200; i++)
                await this.PostAndReadPosterWithStreams();

            // stop stopwatch
            stopWatch.Stop();
            Console.WriteLine($"Elapsed milliseconds with stream on post and read: " +
                $"{stopWatch.ElapsedMilliseconds}, " +
                $"averaging {stopWatch.ElapsedMilliseconds / 200} milliseconds/request");
        }

        /// <summary>
        /// The TestGetPosterWithoutStream
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task TestGetPosterWithoutStream()
        {
            // warmup
            await this.GetPosterWithoutStream();

            // start stopwatch
            Stopwatch stopWatch = Stopwatch.StartNew();

            // run requests
            for (int i = 0; i < 200; i++)
                await this.GetPosterWithoutStream();

            // stop stopwatch
            stopWatch.Stop();
            Console.WriteLine($"Elapsed milliseconds without stream: " +
                $"{stopWatch.ElapsedMilliseconds}, " +
                $"averaging {stopWatch.ElapsedMilliseconds / 200} milliseconds/request");
        }

        /// <summary>
        /// The TestGetPosterWithStream
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task TestGetPosterWithStream()
        {
            // warmup
            await this.GetPosterWithStream();

            // start stopwatch
            Stopwatch stopWatch = Stopwatch.StartNew();

            // run requests
            for (int i = 0; i < 200; i++)
                await this.GetPosterWithStream();

            // stop stopwatch
            stopWatch.Stop();
            Console.WriteLine($"Elapsed milliseconds with stream: " +
                $"{stopWatch.ElapsedMilliseconds}, " +
                $"averaging {stopWatch.ElapsedMilliseconds / 200} milliseconds/request");
        }

        /// <summary>
        /// The TestGetPosterWithStreamAndCompletionMode
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        private async Task TestGetPosterWithStreamAndCompletionMode()
        {
            // warmup
            await this.GetPosterWithStreamAndCompletionMode();

            // start stopwatch
            Stopwatch stopWatch = Stopwatch.StartNew();

            // run requests
            for (int i = 0; i < 200; i++)
                await this.GetPosterWithStreamAndCompletionMode();

            // stop stopwatch
            stopWatch.Stop();
            Console.WriteLine($"Elapsed milliseconds with stream and completionmode: " +
                $"{stopWatch.ElapsedMilliseconds}, " +
                $"averaging {stopWatch.ElapsedMilliseconds / 200} milliseconds/request");
        }
    }
}
