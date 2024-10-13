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
    public class AuctionRepository : IAuctionRepository
    {
        private readonly AppDbContext _context;
        public AuctionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAuctionAsync(AuctionItem auction)
        {
            await _context.AuctionItems.AddAsync(auction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAuctionAsync(AuctionItem auction)
        {
            _context.AuctionItems.Remove(auction); 
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuctionItem>> GetAllAuctionsAsync()
        {
            var auctionsList = await _context.AuctionItems.Include(p => p.CreatedUser).Where(p => !p.IsClosed).OrderBy(p => p.EndDate).ToListAsync();
            return auctionsList;
        }

        public async Task<AuctionItem> GetAuctionByIdAsync(int id)
        {
            var auction = await _context.AuctionItems.Include(a => a.Bids).Include(p => p.CreatedUser).FirstOrDefaultAsync(a => a.Id == id);
            return auction;
        }

        public async Task UpdateAuctionAsync(int id, AuctionItem auction)
        {
            var auctionDb = await _context.AuctionItems.FirstOrDefaultAsync(a => a.Id == id);
            if (auctionDb != null)
            {
                _context.Entry(auction).State = EntityState.Modified;
                //auction.Id = auctionDb.Id;  
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AuctionItem>> GetEndedAuctionsAsync()
        {
            return await _context.AuctionItems
                .Where(a => a.EndDate <= DateTime.Now && !a.IsClosed)
                .ToListAsync();
        }
    }
}
