namespace Movies.API.Services
{
    using Microsoft.EntityFrameworkCore;
    using Movies.API.Contexts;
    using Movies.API.InternalModels;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="PostersRepository" />
    /// </summary>
    public class PostersRepository : IPostersRepository, IDisposable
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private MoviesContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostersRepository"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="MoviesContext"/></param>
        public PostersRepository(MoviesContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// The GetPosterAsync
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="posterId">The posterId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Poster}"/></returns>
        public async Task<Poster> GetPosterAsync(Guid movieId, Guid posterId)
        {
            // Generate the name from the movie title.
            Entities.Movie movie = await this._context.Movies
             .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                throw new Exception($"Movie with id {movieId} not found.");
            }

            // generate a movie poster of 500KB
            Random random = new Random();
            byte[] generatedBytes = new byte[524288];
            random.NextBytes(generatedBytes);

            return new Poster()
            {
                Bytes = generatedBytes,
                Id = posterId,
                MovieId = movieId,
                Name = $"{movie.Title} poster number {DateTime.UtcNow.Ticks}"
            };
        }

        /// <summary>
        /// The AddPoster
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="posterToAdd">The posterToAdd<see cref="Poster"/></param>
        /// <returns>The <see cref="Task{Poster}"/></returns>
        public async Task<Poster> AddPoster(Guid movieId, Poster posterToAdd)
        {
            // don't do anything: we're just faking this.  Simply return the poster
            // after setting the ids
            posterToAdd.MovieId = movieId;
            posterToAdd.Id = Guid.NewGuid();
            return await Task.FromResult(posterToAdd);
        }

        /// <summary>
        /// The Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The Dispose
        /// </summary>
        /// <param name="disposing">The disposing<see cref="bool"/></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
        }
    }
}
