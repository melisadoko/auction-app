using Auction.Core.Entities;
using Auction.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IdentityResult> RegisterUserAsync(string username, string firstName, string lastName, string password)
        {
            var existingUser = await _userManager.FindByNameAsync(username);
            if (existingUser != null)
            {
                _logger.LogWarning($"Attempt to register user '{username}' failed: user already exists.");
                return IdentityResult.Failed(new IdentityError { Description = "User already exists." });
            }

            var user = new User { UserName = username, FirstName = firstName, LastName = lastName };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User '{username}' registered successfully.");
            }
            else
            {
                _logger.LogError($"Failed to register user '{username}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
            return result;
        }

        public async Task<bool> LoginUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                _logger.LogWarning($"Login attempt failed: user '{username}' not found.");
                return false;
            }
            var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User '{username}' logged in successfully.");
            }
            else
            {
                _logger.LogWarning($"Login attempt for user '{username}' failed: {(result.IsLockedOut ? "Account locked out" : "Invalid password")}.");
            }

            return result.Succeeded;
        }

        public async Task LogoutUserAsync()
        {
            _logger.LogInformation("User logging out.");
            await _signInManager.SignOutAsync();
        }

    }
}
