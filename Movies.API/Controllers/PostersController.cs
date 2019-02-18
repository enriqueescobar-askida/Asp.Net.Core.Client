namespace Movies.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Movies.API.InternalModels;
    using Movies.API.Services;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="PostersController" />
    /// </summary>
    [Route("api/movies/{movieId}/posters")]
    [ApiController]
    public class PostersController : ControllerBase
    {
        /// <summary>
        /// Defines this postersRepository
        /// </summary>
        private readonly IPostersRepository postersRepository;

        /// <summary>
        /// Defines the mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostersController"/> class.
        /// </summary>
        /// <param name="iPostersRepository">The postersRepository<see cref="IPostersRepository"/></param>
        /// <param name="iMapper">The mapper<see cref="IMapper"/></param>
        public PostersController(IPostersRepository iPostersRepository, IMapper iMapper)
        {
            this.postersRepository = iPostersRepository ?? throw new ArgumentNullException(nameof(iPostersRepository));
            this.mapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
        }

        /// <summary>
        /// The GetPoster
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="posterId">The posterId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{ActionResult{Models.Poster}}"/></returns>
        [HttpGet("{posterId}", Name = "GetPoster")]
        public async Task<ActionResult<Models.Poster>> GetPoster(Guid movieId, Guid posterId)
        {
            Poster poster = await this.postersRepository.GetPosterAsync(movieId, posterId);

            if (poster == null)
                return NotFound();

            return Ok(this.mapper.Map<Models.Poster>(poster));
        }

        /// <summary>
        /// The CreatePoster
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="posterForCreation">The posterForCreation<see cref="Models.PosterForCreation"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost]
        public async Task<IActionResult> CreatePoster(Guid movieId, [FromBody] Models.PosterForCreation posterForCreation)
        {
            // model validation
            if (posterForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                // return 422 - Unprocessable Entity when validation fails
                return new UnprocessableEntityObjectResult(ModelState);

            Poster poster = this.mapper.Map<Poster>(posterForCreation);
            Poster createdPoster = await this.postersRepository.AddPoster(movieId, poster);

            // no need to save, in this type of repo the poster is
            // immediately persisted.

            // map the poster from the repository to a shared model poster
            return CreatedAtRoute("GetPoster",
                new { movieId, posterId = createdPoster.Id },
                this.mapper.Map<Models.Poster>(createdPoster));
        }
    }
}
