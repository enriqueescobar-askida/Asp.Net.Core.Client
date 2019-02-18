namespace Movies.API.Services
{
    using Microsoft.EntityFrameworkCore;
    using Movies.API.Contexts;
    using Movies.API.InternalModels;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="TrailersRepository" />
    /// </summary>
    public class TrailersRepository : ITrailersRepository, IDisposable
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private MoviesContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrailersRepository"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="MoviesContext"/></param>
        public TrailersRepository(MoviesContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// The GetTrailerAsync
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="trailerId">The trailerId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Trailer}"/></returns>
        public async Task<Trailer> GetTrailerAsync(Guid movieId, Guid trailerId)
        {
            // Generate the name from the movie title.
            Entities.Movie movie = await this._context.Movies
             .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
                throw new Exception($"Movie with id {movieId} not found.");

            // generate a trailer (byte array) between 50 and 100MB
            Random random = new Random();
            int generatedByteLength = random.Next(52428800, 104857600);
            byte[] generatedBytes = new byte[generatedByteLength];
            random.NextBytes(generatedBytes);

            return new Trailer()
            {
                Bytes = generatedBytes,
                Id = trailerId,
                MovieId = movieId,
                Name = $"{movie.Title} trailer number {DateTime.UtcNow.Ticks}",
                Description = $"{movie.Title} trailer description {DateTime.UtcNow.Ticks}"
            };
        }

        /// <summary>
        /// The AddTrailer
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="trailerToAdd">The trailerToAdd<see cref="Trailer"/></param>
        /// <returns>The <see cref="Task{Trailer}"/></returns>
        public async Task<Trailer> AddTrailer(Guid movieId, Trailer trailerToAdd)
        {
            // don't do anything: we're just faking this.  Simply return the trailer
            // after setting the ids
            trailerToAdd.MovieId = movieId;
            trailerToAdd.Id = Guid.NewGuid();
            return await Task.FromResult(trailerToAdd);
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
