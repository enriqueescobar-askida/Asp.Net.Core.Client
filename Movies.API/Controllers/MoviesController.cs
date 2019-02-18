namespace Movies.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Movies.API.Entities;
    using Movies.API.Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="MoviesController" />
    /// </summary>
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        /// <summary>
        /// Defines the moviesRepository
        /// </summary>
        private readonly IMoviesRepository moviesRepository;

        /// <summary>
        /// Defines the mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MoviesController"/> class.
        /// </summary>
        /// <param name="iMoviesRepository">The moviesRepository<see cref="IMoviesRepository"/></param>
        /// <param name="imapper">The mapper<see cref="IMapper"/></param>
        public MoviesController(IMoviesRepository iMoviesRepository, IMapper imapper)
        {
            this.moviesRepository = iMoviesRepository ?? throw new ArgumentNullException(nameof(iMoviesRepository));
            this.mapper = imapper ?? throw new ArgumentNullException(nameof(imapper));
        }

        /// <summary>
        /// The GetMovies
        /// </summary>
        /// <returns>The <see cref="Task{ActionResult{IEnumerable{Models.Movie}}}"/></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Movie>>> GetMovies()
        {
            IEnumerable<Movie> movieEntities = await this.moviesRepository.GetMoviesAsync();

            return this.Ok(this.mapper.Map<IEnumerable<Models.Movie>>(movieEntities));
        }

        /// <summary>
        /// The GetMovie
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{ActionResult{Models.Movie}}"/></returns>
        [HttpGet("{movieId}", Name = "GetMovie")]
        public async Task<ActionResult<Models.Movie>> GetMovie(Guid movieId)
        {
            Movie movieEntity = await this.moviesRepository.GetMovieAsync(movieId);

            if (movieEntity == null)
                return this.NotFound();

            return this.Ok(this.mapper.Map<Models.Movie>(movieEntity));
        }

        /// <summary>
        /// The CreateMovie
        /// </summary>
        /// <param name="movieForCreation">The movieForCreation<see cref="Models.MovieForCreation"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] Models.MovieForCreation movieForCreation)
        {
            // model validation
            if (movieForCreation == null)
                return this.BadRequest();

            if (!this.ModelState.IsValid)
                // return 422 - Unprocessable Entity when validation fails
                return new UnprocessableEntityObjectResult(this.ModelState);

            Movie movieEntity = this.mapper.Map<Movie>(movieForCreation);
            this.moviesRepository.AddMovie(movieEntity);

            // save the changes
            await this.moviesRepository.SaveChangesAsync();

            // Fetch the movie from the data store so the director is included
            await this.moviesRepository.GetMovieAsync(movieEntity.Id);

            return this.CreatedAtRoute("GetMovie",
                new { movieId = movieEntity.Id },
                this.mapper.Map<Models.Movie>(movieEntity));
        }

        /// <summary>
        /// The UpdateMovie
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="movieForUpdate">The movieForUpdate<see cref="Models.MovieForUpdate"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPut("{movieId}")]
        public async Task<IActionResult> UpdateMovie(Guid movieId, [FromBody] Models.MovieForUpdate movieForUpdate)
        {
            // model validation
            if (!this.ModelState.IsValid)
                // return 422 - Unprocessable Entity when validation fails
                return new UnprocessableEntityObjectResult(this.ModelState);

            Movie movieEntity = await this.moviesRepository.GetMovieAsync(movieId);

            if (movieEntity == null)
                return this.NotFound();

            // map the inputted object into the movie entity
            // this ensures properties will get updated
            this.mapper.Map(movieForUpdate, movieEntity);

            // call into UpdateMovie even though in our implementation
            // this doesn't contain code - doing this ensures the code stays
            // reliable when other repository implementations (e.g.: a mock
            // repository) are used.
            this.moviesRepository.UpdateMovie(movieEntity);
            await this.moviesRepository.SaveChangesAsync();

            // return the updated movie, after mapping it
            return this.Ok(this.mapper.Map<Models.Movie>(movieEntity));
        }

        /// <summary>
        /// The PartiallyUpdateMovie
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="patchDoc">The patchDoc<see cref="JsonPatchDocument{Models.MovieForUpdate}"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPatch("{movieId}")]
        public async Task<IActionResult> PartiallyUpdateMovie(Guid movieId, [FromBody] JsonPatchDocument<Models.MovieForUpdate> patchDoc)
        {
            Movie movieEntity = await this.moviesRepository.GetMovieAsync(movieId);

            if (movieEntity == null)
                return this.NotFound();

            // the patch is on a DTO, not on the movie entity
            Models.MovieForUpdate movieToPatch = Mapper.Map<Models.MovieForUpdate>(movieEntity);

            patchDoc.ApplyTo(movieToPatch, this.ModelState);

            if (!this.ModelState.IsValid)
                return new UnprocessableEntityObjectResult(this.ModelState);

            // map back to the entity, and save
            Mapper.Map(movieToPatch, movieEntity);

            // call into UpdateMovie even though in our implementation
            // this doesn't contain code - doing this ensures the code stays
            // reliable when other repository implementations (eg: a mock repository) are used.
            this.moviesRepository.UpdateMovie(movieEntity);
            await this.moviesRepository.SaveChangesAsync();

            // return the updated movie, after mapping it
            return this.Ok(this.mapper.Map<Models.Movie>(movieEntity));
        }

        /// <summary>
        /// The DeleteMovie
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpDelete("{movieid}")]
        public async Task<IActionResult> DeleteMovie(Guid movieId)
        {
            Movie movieEntity = await this.moviesRepository.GetMovieAsync(movieId);

            if (movieEntity == null)
                return this.NotFound();

            this.moviesRepository.DeleteMovie(movieEntity);
            await this.moviesRepository.SaveChangesAsync();

            return this.NoContent();
        }
    }
}
