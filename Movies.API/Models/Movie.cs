namespace Movies.API.Models
{
    using System;

    /// <summary>
    /// Defines the <see cref="Movie" />
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Genre
        /// </summary>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the ReleaseDate
        /// </summary>
        public DateTimeOffset ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the Director
        /// </summary>
        public string Director { get; set; }
    }
}
