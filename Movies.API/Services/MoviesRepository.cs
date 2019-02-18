namespace Movies.API.Services
{
    using Microsoft.EntityFrameworkCore;
    using Movies.API.Contexts;
    using Movies.API.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="MoviesRepository" />
    /// </summary>
    public class MoviesRepository : IMoviesRepository, IDisposable
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private MoviesContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoviesRepository"/> class.
        /// </summary>
        /// <param name="context">The context<see cref="MoviesContext"/></param>
        public MoviesRepository(MoviesContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// The GetMovieAsync
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Movie}"/></returns>
        public async Task<Movie> GetMovieAsync(Guid movieId)
        {
            return await this._context.Movies.Include(m => m.Director)
                .FirstOrDefaultAsync(m => m.Id == movieId);
        }

        /// <summary>
        /// The GetMoviesAsync
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{Movie}}"/></returns>
        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await this._context.Movies.Include(m => m.Director).ToListAsync();
        }

        /// <summary>
        /// The UpdateMovie
        /// </summary>
        /// <param name="movieToUpdate">The movieToUpdate<see cref="Movie"/></param>
        public void UpdateMovie(Movie movieToUpdate)
        {
        }

        /// <summary>
        /// The AddMovie
        /// </summary>
        /// <param name="movieToAdd">The movieToAdd<see cref="Movie"/></param>
        public void AddMovie(Movie movieToAdd)
        {
            if (movieToAdd == null)
                throw new ArgumentNullException(nameof(movieToAdd));

            this._context.Add(movieToAdd);
        }

        /// <summary>
        /// The DeleteMovie
        /// </summary>
        /// <param name="movieToDelete">The movieToDelete<see cref="Movie"/></param>
        public void DeleteMovie(Movie movieToDelete)
        {
            if (movieToDelete == null)
                throw new ArgumentNullException(nameof(movieToDelete));

            this._context.Remove(movieToDelete);
        }

        /// <summary>
        /// The SaveChangesAsync
        /// </summary>
        /// <returns>The <see cref="Task{bool}"/></returns>
        public async Task<bool> SaveChangesAsync()
        {
            // return true if 1 or more entities were changed
            return (await this._context.SaveChangesAsync() > 0);
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
            {
                if (this._context != null)
                {
                    this._context.Dispose();
                    this._context = null;
                }
            }
        }
    }
}
