namespace Movies.API.InternalModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="Trailer" />
    /// </summary>
    public class Trailer
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
        /// Gets or sets the Description
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Bytes
        /// </summary>
        [Required]
        public byte[] Bytes { get; set; }
    }
}
