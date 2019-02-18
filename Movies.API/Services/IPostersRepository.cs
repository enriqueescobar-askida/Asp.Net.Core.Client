namespace Movies.API.Services
{
    using Movies.API.InternalModels;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IPostersRepository" />
    /// </summary>
    public interface IPostersRepository
    {
        /// <summary>
        /// The GetPosterAsync
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="posterId">The posterId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{Poster}"/></returns>
        Task<Poster> GetPosterAsync(Guid movieId, Guid posterId);

        /// <summary>
        /// The AddPoster
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="posterToAdd">The posterToAdd<see cref="Poster"/></param>
        /// <returns>The <see cref="Task{Poster}"/></returns>
        Task<Poster> AddPoster(Guid movieId, Poster posterToAdd);
    }
}
