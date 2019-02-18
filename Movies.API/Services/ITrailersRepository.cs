namespace Movies.API.Services
{
    using Movies.API.InternalModels;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="ITrailersRepository" />
    /// </summary>
    public interface ITrailersRepository
    {
        /// <summary>
        /// The GetTrailerAsync
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="trailerId">The trailerId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Trailer}"/></returns>
        Task<Trailer> GetTrailerAsync(Guid movieId, Guid trailerId);

        /// <summary>
        /// The AddTrailer
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="trailerToAdd">The trailerToAdd<see cref="Trailer"/></param>
        /// <returns>The <see cref="Task{Trailer}"/></returns>
        Task<Trailer> AddTrailer(Guid movieId, Trailer trailerToAdd);
    }
}
