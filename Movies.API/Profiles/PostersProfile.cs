namespace Movies.API.Profiles
{
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile for working with Poster objects
    /// </summary>
    public class PostersProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostersProfile"/> class.
        /// </summary>
        public PostersProfile()
        {
            this.CreateMap<InternalModels.Poster, Models.Poster>();
            this.CreateMap<Models.PosterForCreation, InternalModels.Poster>();
        }
    }
}
