namespace Movies.API.Profiles
{
    using AutoMapper;

    /// <summary>
    /// AutoMapper profile for working with Trailer objects
    /// </summary>
    public class TrailersProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrailersProfile"/> class.
        /// </summary>
        public TrailersProfile()
        {
            this.CreateMap<InternalModels.Trailer, Models.Trailer>();
            this.CreateMap<Models.TrailerForCreation, InternalModels.Trailer>();
        }
    }
}
