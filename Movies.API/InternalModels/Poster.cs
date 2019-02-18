namespace Movies.API.InternalModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="Poster" />
    /// </summary>
    public class Poster
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the MovieId
        /// </summary>
        [Required]
        public Guid MovieId { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Bytes
        /// </summary>
        [Required]
        public byte[] Bytes { get; set; }
    }
}
