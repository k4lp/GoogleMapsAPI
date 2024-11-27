﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoogleMapsAPI.Models.GoogleMapsApiServiceModels
{
    /// <summary>
    /// Represents a business or place result from Google Maps API.
    /// </summary>
    public class GoogleMapsResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary Key for EF Core

        [MaxLength(100)]
        public string? BusinessStatus { get; set; } // Follows PascalCase naming convention

        [Required]
        [MaxLength(200)]
        public string? Name { get; set; }

        [MaxLength(100)]
        public string? PlaceId { get; set; }

        [MaxLength(300)]
        public string? Vicinity { get; set; }
    }

    /// <summary>
    /// Represents the root object returned by the Google Maps API.
    /// </summary>
    public class GoogleMapsRoot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary Key for EF Core

        [MaxLength(50)]
        public string? Status { get; set; }

        [MaxLength(500)]
        public string? NextPageToken { get; set; }

        // Establishes one-to-many relationship with Result
        public List<GoogleMapsResult>? Results { get; set; }
    }
}
