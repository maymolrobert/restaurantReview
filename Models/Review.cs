using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantReviews.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required, StringLength(60, MinimumLength = 3)]
        public string? RestaurantName { get; set; }

        [Required, StringLength(60, MinimumLength = 3)]
        public string? FoodName { get; set; }

        [Required, StringLength(300, MinimumLength = 3)]
        public string? Description { get; set; }

        [Required, StringLength(300, MinimumLength = 3)]
        public string? UserName { get; set; }

        [Required, Range(0.01, 9999999.99)]
        public decimal Price { get; set; }

        [Required, Range(0, 5)]
        public int Score { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public byte[]? Image { get; set; }

        [NotMapped] // This property will not be saved to the database
        public IFormFile? ImageFile { get; set; }
    }
}