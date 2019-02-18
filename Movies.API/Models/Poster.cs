namespace Movies.API.Models
{
    using System;

    /// <summary>
    /// Defines the <see cref="Poster" />
    /// </summary>
    public class Poster
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the MovieId
        /// </summary>
        public Guid MovieId { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Bytes
        /// </summary>
        public byte[] Bytes { get; set; }
    }
}
