using Microsoft.EntityFrameworkCore;
using RestaurantReviews.Models;

namespace RestaurantReviews.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public required DbSet<Review> Reviews { get; set; }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use SQLite if no provider has been configured (only necessary if dependency injection isn't being used)
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=RestaurantReviews.db");
            }

            // Suppress specific warnings
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            byte[] bbribs = File.ReadAllBytes("./images/bbqRibs.jpeg");
            byte[] beefTacos = File.ReadAllBytes("./images/beefTacos.jpeg");
            byte[] cheeseburger = File.ReadAllBytes("./images/cheeseBurger.jpeg");
            byte[] chocolateCake = File.ReadAllBytes("./images/chocolateCake.jpeg");
            byte[] margheritaPizza = File.ReadAllBytes("./images/margheritaPizza.jpeg");
            byte[] padThai = File.ReadAllBytes("./images/padThai.jpeg");
            byte[] QuinoaSalad = File.ReadAllBytes("./images/QuinoaSalad.jpeg");
            byte[] sashimiPlatter = File.ReadAllBytes("./images/sashimiPlatter.jpeg");
            byte[] spaghettiCarbonara = File.ReadAllBytes("./images/spaghettiCarbonara.jpeg");
            byte[] steak = File.ReadAllBytes("./images/steak.jpeg");

            // SQLite-specific adjustments
            modelBuilder.Entity<Review>()
                .Property(r => r.Price)
                .HasColumnType("decimal(9, 2)"); // SQLite doesn't support `HasPrecision`, so we specify type

            // Seeding data
            modelBuilder.Entity<Review>().HasData(
                new Review
                {
                    Id = 1,
                    RestaurantName = "The Keg",
                    FoodName = "Steak",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 45.99m,
                    Score = 5,
                    PublishDate = DateTime.Now.AddDays(-5),
                    Image = steak // Placeholder
                },
                new Review
                {
                    Id = 2,
                    RestaurantName = "Pasta Palace",
                    FoodName = "Spaghetti Carbonara",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 25.99m,
                    Score = 4,
                    PublishDate = DateTime.Now.AddDays(-10),
                    Image = spaghettiCarbonara // Placeholder
                },
                new Review
                {
                    Id = 3,
                    RestaurantName = "Burger Haven",
                    FoodName = "Cheeseburger",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 15.49m,
                    Score = 3,
                    PublishDate = DateTime.Now.AddDays(-7),
                    Image = cheeseburger // Placeholder
                },
                new Review
                {
                    Id = 4,
                    RestaurantName = "Sushi Central",
                    FoodName = "Sashimi Platter",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 35.99m,
                    Score = 5,
                    PublishDate = DateTime.Now.AddDays(-3),
                    Image = sashimiPlatter // Placeholder
                },
                new Review
                {
                    Id = 5,
                    RestaurantName = "Pizza Kingdom",
                    FoodName = "Margherita Pizza",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 20.00m,
                    Score = 4,
                    PublishDate = DateTime.Now.AddDays(-8),
                    Image = margheritaPizza // Placeholder
                },
                new Review
                {
                    Id = 6,
                    RestaurantName = "Taco Town",
                    FoodName = "Beef Tacos",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 12.99m,
                    Score = 3,
                    PublishDate = DateTime.Now.AddDays(-12),
                    Image = beefTacos // Placeholder
                },
                new Review
                {
                    Id = 7,
                    RestaurantName = "Noodle Nirvana",
                    FoodName = "Pad Thai",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 18.99m,
                    Score = 5,
                    PublishDate = DateTime.Now.AddDays(-4),
                    Image = padThai // Placeholder
                },
                new Review
                {
                    Id = 8,
                    RestaurantName = "Grill Master",
                    FoodName = "BBQ Ribs",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 30.00m,
                    Score = 4,
                    PublishDate = DateTime.Now.AddDays(-9),
                    Image = bbribs // Placeholder
                },
                new Review
                {
                    Id = 9,
                    RestaurantName = "Dessert Delights",
                    FoodName = "Chocolate Cake",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 8.99m,
                    Score = 4,
                    PublishDate = DateTime.Now.AddDays(-1),
                    Image = chocolateCake // Placeholder
                },
                new Review
                {
                    Id = 10,
                    RestaurantName = "Vegan Bliss",
                    FoodName = "Quinoa Salad",
                    UserName = "Quinoa Salad",
                    Description = "Quinoa Salad",
                    Price = 16.50m,
                    Score = 5,
                    PublishDate = DateTime.Now.AddDays(-6),
                    Image = QuinoaSalad // Placeholder
                }
            );
        }
    }
}
