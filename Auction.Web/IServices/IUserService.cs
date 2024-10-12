using Auction.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Core.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterUserAsync(string username, string firstName, string lastName, string password);
        Task<bool> LoginUserAsync(string username, string password);
        Task LogoutUserAsync();
    }
}
