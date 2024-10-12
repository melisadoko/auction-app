using Microsoft.AspNetCore.Mvc;

namespace Auction.Web.Controllers
{
    using Auction.Core.Interfaces;
    using Auction.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model.Username, model.FirstName, model.LastName, model.Password);
                if (result.Succeeded)
                {
                    await _userService.LoginUserAsync(model.Username, model.Password);
                    return RedirectToAction("Index", "Auction");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // GET: /Account/Login
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isLoggedIn = await _userService.LoginUserAsync(model.Username, model.Password);
                if (isLoggedIn)
                {
                    return RedirectToAction("Index", "Auction");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUserAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
