namespace Movies.API.Services
{
    using Movies.API.Entities;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IMoviesRepository" />
    /// </summary>
    public interface IMoviesRepository
    {
        /// <summary>
        /// The GetMovieAsync
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Movie}"/></returns>
        Task<Movie> GetMovieAsync(Guid movieId);

        /// <summary>
        /// The GetMoviesAsync
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{Movie}}"/></returns>
        Task<IEnumerable<Movie>> GetMoviesAsync();

        /// <summary>
        /// The UpdateMovie
        /// </summary>
        /// <param name="movieToUpdate">The movieToUpdate<see cref="Movie"/></param>
        void UpdateMovie(Movie movieToUpdate);

        /// <summary>
        /// The AddMovie
        /// </summary>
        /// <param name="movieToAdd">The movieToAdd<see cref="Movie"/></param>
        void AddMovie(Movie movieToAdd);

        /// <summary>
        /// The DeleteMovie
        /// </summary>
        /// <param name="movieToDelete">The movieToDelete<see cref="Movie"/></param>
        void DeleteMovie(Movie movieToDelete);

        /// <summary>
        /// The SaveChangesAsync
        /// </summary>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> SaveChangesAsync();
    }
}
