namespace Movies.API.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Defines the <see cref="Director" />
    /// </summary>
    [Table("Directors")]
    public class Director
    {
        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the FirstName
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }
    }
}
