using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoogleMapsAPI.Models.Entities.GoogleMaps
{
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
