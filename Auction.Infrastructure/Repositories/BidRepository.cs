using Auction.Core.Entities;
using Auction.Infrastructure.Data;
using Auction.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Infrastructure.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly AppDbContext _context;

        public BidRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddBidAsync(Bid bid)
        {
            await _context.Bids.AddAsync(bid);
            await _context.SaveChangesAsync();
        }

        public async Task<Bid> GetHighestBidForAuctionAsync(int auctionId)
        {
            var highestBid = await _context.Bids.Include(p => p.User)
                        .Where(b => b.AuctionId == auctionId)
                        .OrderByDescending(b => b.Amount)
                        .FirstOrDefaultAsync();
            return highestBid;
        }
    }
}
