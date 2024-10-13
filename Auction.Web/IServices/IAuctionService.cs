using Auction.Web.Models;

namespace Auction.Web.IServices
{
    public interface IAuctionService
    {
        Task<IEnumerable<AuctionViewModel>> GetAllAuctionsAsync();
        Task<AuctionViewModel> GetAuctionDetailsAsync(int id);
        Task PlaceBidAsync(BidViewModel bidModel, string userId);
        Task DeleteAuctionAsync(int auctionId, string userId);
        Task AddAuctionAsync(AuctionViewModel auctionViewModel, string userId);
        Task CheckEndAuctionsAsync();
    }
}
