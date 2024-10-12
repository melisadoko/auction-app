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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<AuctionItem>()
                .HasOne(a => a.CreatedUser)
                .WithMany() 
                .HasForeignKey(a => a.UserCreatedId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.User)
                .WithMany() 
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
        public DbSet<AuctionItem> AuctionItems { get; set; }
        public DbSet<Bid> Bids { get; set; }
    }
}
