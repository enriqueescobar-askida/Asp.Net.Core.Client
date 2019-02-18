namespace Movies.API
{
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile for working with Movie objects
    /// </summary>
    public class MoviesProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoviesProfile"/> class.
        /// </summary>
        public MoviesProfile()
        {
            this.CreateMap<Entities.Movie, Models.Movie>()
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src =>
                   $"{src.Director.FirstName} {src.Director.LastName}"));

            this.CreateMap<Models.MovieForCreation, Entities.Movie>();

            this.CreateMap<Models.MovieForUpdate, Entities.Movie>().ReverseMap();
        }
    }
}
