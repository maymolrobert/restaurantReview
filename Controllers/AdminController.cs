using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RestaurantReviews.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantReviews.Controllers
{
    public class AdminController : Controller
    {
        private const string AdminUsername = "ConestogaCollege.123";
        private const string AdminPassword = "ConestogaCollege.123";

        // Admin Login View
        public IActionResult Login() => View();

        // Admin Login Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Admin admin)
        {
            // Validate credentials
            if (admin.Username == AdminUsername && admin.Password == AdminPassword)
            {
                // Create claims for the admin role
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, admin.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sign the user in
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                return RedirectToAction("Index", "Reviews"); // Redirect to Reviews page after login
            }

            ViewBag.Error = "Invalid Credentials";
            return View();
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
