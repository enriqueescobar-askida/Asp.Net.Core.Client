namespace Movies.API.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Defines the <see cref="PosterForCreation" />
    /// </summary>
    public class PosterForCreation
    {
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
