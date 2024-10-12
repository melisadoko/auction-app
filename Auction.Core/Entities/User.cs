using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace Auction.Core.Entities
{
    public class User: IdentityUser
    {
        public decimal WalletBalance { get; set; } = 1000; 
        public string FirstName {get; set; }
        public string LastName {get; set; }
    }
}
