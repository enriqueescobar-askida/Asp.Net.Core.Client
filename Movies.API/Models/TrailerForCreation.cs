namespace Movies.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="TrailerForCreation" />
    /// </summary>
    public class TrailerForCreation
    {
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
