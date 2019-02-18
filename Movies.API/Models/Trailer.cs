namespace Movies.API.Models
{
    using System;

    /// <summary>
    /// Defines the <see cref="Trailer" />
    /// </summary>
    public class Trailer
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
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Bytes
        /// </summary>
        public byte[] Bytes { get; set; }
    }
}
