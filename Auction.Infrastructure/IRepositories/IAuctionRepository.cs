using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Core.Entities;
namespace Auction.Infrastructure.IRepositories
{
    public interface IAuctionRepository
    {
        Task<List<AuctionItem>> GetAllAuctionsAsync();
        Task<AuctionItem> GetAuctionByIdAsync(int id);
        Task AddAuctionAsync(AuctionItem auction);
        Task DeleteAuctionAsync(AuctionItem auction);
    }
}
