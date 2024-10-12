using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Auction.Core.Entities;

namespace Auction.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<AuctionItem> AuctionItems { get; set; }
        //public DbSet<Bid> Bids { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder); // Call base method for Identity

        //    // Configure other entities here (if needed)
        //    modelBuilder.Entity<AuctionItem>()
        //        .HasOne(a => a.Owner)
        //    .WithMany()
        //        .HasForeignKey(a => a.OwnerId);

        //    modelBuilder.Entity<Bid>()
        //        .HasOne<User>().WithMany().HasForeignKey(b => b.BidderId);
        //}
    }
}
