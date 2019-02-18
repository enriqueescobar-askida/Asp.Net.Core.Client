namespace Movies.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="MovieForCreation" />
    /// </summary>
    public class MovieForCreation
    {
        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        [MaxLength(2000)]
        [MinLength(10)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Genre
        /// </summary>
        [MaxLength(200)]
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the ReleaseDate
        /// </summary>
        [Required]
        public DateTimeOffset? ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the DirectorId
        /// </summary>
        [Required]
        public Guid? DirectorId { get; set; }
    }
}
