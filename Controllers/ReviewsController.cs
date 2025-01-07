using System.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantReviews.Data;
using RestaurantReviews.Models;

namespace RestaurantReviews.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reviews.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Search(string searchQuery)
        {
            var reviews = from r in _context.Reviews
                          select r;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                reviews = reviews.Where(r =>
                    r.RestaurantName!.Contains(searchQuery) ||
                    r.FoodName!.Contains(searchQuery) ||
                    r.UserName!.Contains(searchQuery) ||
                    r.Description!.Contains(searchQuery));
            }

            return View("Index", await reviews.ToListAsync());
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RestaurantName,FoodName,UserName,Description,Price,Score,PublishDate,ImageFile")] Review review)
        {
            try
            {
                Console.WriteLine("review.Imag ========== " + review.Image);
                Console.WriteLine("review.ImagFile ========== " + review.ImageFile);
                if (review.ImageFile != null && review.ImageFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await review.ImageFile.CopyToAsync(memoryStream);
                        review.Image = memoryStream.ToArray(); // Store the byte array in the Image property
                    }
                }

                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to the index view after successful save
            }
            catch (Exception ex)
            {
                // Log the exception (you could use a logging framework like Serilog)
                ModelState.AddModelError("", $"An error occurred while saving the review: {ex.Message}");
            }

            // If we reach here, something went wrong, so return to the same view with the review and validation errors
            return View(review);
        }


        // GET: Reviews/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        // POST: Reviews/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RestaurantName,FoodName,UserName,Description,Price,Score,PublishDate,ImageFile")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine("review.Imag ========== " + review.Image);
                Console.WriteLine("review.ImagFile ========== " + review.ImageFile);
                    if (review.ImageFile != null && review.ImageFile.Length > 0)
                    {
                        Console.WriteLine("inside if");
                        using (var memoryStream = new MemoryStream())
                        {
                            await review.ImageFile.CopyToAsync(memoryStream);
                            review.Image = memoryStream.ToArray(); // Store the byte array in the Image property
                        }
                    }
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        // GET: Reviews/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            // Check if admin session exists
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }

        // GET: Admin Login
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        // POST: Admin Login
        // POST: Admin Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminLogin(string username, string password)
        {
            // Simple check for admin credentials
            Console.WriteLine("reviews Before username password check" + username + "==========" + password);
            if (username == "ConestogaCollege.123" && password == "ConestogaCollege.123")
            {
                Console.WriteLine("Before username password check" + username + "==========" + password);
                Console.WriteLine("setting true");
                // If login is successful, set a session to recognize the admin user
                HttpContext.Session.SetString("Admin", "true");

                Console.WriteLine("HttpContext.Session.GetString" + HttpContext.Session.GetString("Admin"));

                // Optionally, you can redirect the user to the page they were initially trying to access (if applicable)
                return RedirectToAction(nameof(Index));
            }

            // Invalid login attempt
            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }


        // GET: Admin Logout
        [HttpGet]
        public async Task<IActionResult> AdminLogout()
        {
            // Remove the session value when logging out
            HttpContext.Session.Remove("Admin");

            // Optionally, clear authentication cookies if you're using cookie-based auth
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }
    }
}
