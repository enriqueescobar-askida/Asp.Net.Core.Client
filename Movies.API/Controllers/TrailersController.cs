namespace Movies.API.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Movies.API.InternalModels;
    using Movies.API.Services;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="TrailersController" />
    /// </summary>
    [Route("api/movies/{movieId}/trailers")]
    [ApiController]
    public class TrailersController : ControllerBase
    {
        /// <summary>
        /// Defines the _trailersRepository
        /// </summary>
        private readonly ITrailersRepository trailersRepository;

        /// <summary>
        /// Defines the _mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrailersController"/> class.
        /// </summary>
        /// <param name="iTrailersRepository">The trailersRepository<see cref="ITrailersRepository"/></param>
        /// <param name="iMapper">The mapper<see cref="IMapper"/></param>
        public TrailersController(ITrailersRepository iTrailersRepository, IMapper iMapper)
        {
            this.trailersRepository = iTrailersRepository ?? throw new ArgumentNullException(nameof(iTrailersRepository));
            this.mapper = iMapper ?? throw new ArgumentNullException(nameof(iMapper));
        }

        /// <summary>
        /// The GetTrailer
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="trailerId">The trailerId<see cref="Guid"/></param>
        /// <returns>The <see cref="Task{ActionResult{Models.Trailer}}"/></returns>
        [HttpGet("{trailerId}", Name = "GetTrailer")]
        public async Task<ActionResult<Models.Trailer>> GetTrailer(Guid movieId, Guid trailerId)
        {
            Trailer trailer = await this.trailersRepository.GetTrailerAsync(movieId, trailerId);

            if (trailer == null)
                return NotFound();

            return Ok(this.mapper.Map<Models.Trailer>(trailer));
        }

        /// <summary>
        /// The CreateTrailer
        /// </summary>
        /// <param name="movieId">The movieId<see cref="Guid"/></param>
        /// <param name="trailerForCreation">The trailerForCreation<see cref="Models.TrailerForCreation"/></param>
        /// <returns>The <see cref="Task{IActionResult}"/></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTrailer(Guid movieId, [FromBody] Models.TrailerForCreation trailerForCreation)
        {
            // model validation
            if (trailerForCreation == null)
                return BadRequest();

            if (!ModelState.IsValid)
                // return 422 - Unprocessable Entity when validation fails
                return new UnprocessableEntityObjectResult(ModelState);

            Trailer trailer = this.mapper.Map<Trailer>(trailerForCreation);
            Trailer createdTrailer = await this.trailersRepository.AddTrailer(movieId, trailer);

            // no need to save, in this type of repo the trailer is
            // immediately persisted.

            // map the trailer from the repository to a shared model trailer
            return CreatedAtRoute("GetTrailer",
                new { movieId, trailerId = createdTrailer.Id },
                this.mapper.Map<Models.Trailer>(createdTrailer));
        }
    }
}
