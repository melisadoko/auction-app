using Auction.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Infrastructure.IRepositories
{
    public interface IBidRepository
    {
        Task AddBidAsync(Bid bid);
        Task<Bid> GetHighestBidForAuctionAsync(int auctionId);
    }
}
