using Auction.Core.Entities;
using Auction.Infrastructure.IRepositories;
using Auction.Web.IServices;
using Auction.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auction.Web.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuctionService> _logger;

        public AuctionService(IAuctionRepository auctionRepository, IBidRepository bidRepository, UserManager<User> userManager, ILogger<AuctionService> logger)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task DeleteAuctionAsync(int auctionId, string userId)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(auctionId);
            if (auction == null || auction.UserCreatedId != userId)
            {
                _logger.LogWarning("Unauthorized delete attempt for auction ID {AuctionId} by user ID {UserId}.", auctionId, userId);
                throw new InvalidOperationException("You are not authorized to delete this auction.");
            }

            try
            {
                // Call the repository method to delete the auction
                await _auctionRepository.DeleteAuctionAsync(auction);
                _logger.LogInformation("Auction ID {AuctionId} deleted by user ID {UserId}.", auctionId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting auction ID {AuctionId}.", auctionId);
                throw new Exception("There was an error deleting the auction. Please try again.");
            }
        }


        public async Task<IEnumerable<AuctionViewModel>> GetAllAuctionsAsync()
        {
            _logger.LogInformation("Fetching all auctions.");

            try
            {
                var auctions = await _auctionRepository.GetAllAuctionsAsync();
                var auctionViewModels = auctions.Select(a => new AuctionViewModel()
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    CurrentPrice = a.CurrentPrice,
                    EndDate = a.EndDate,
                    UserId = a.UserCreatedId,
                    CreatedUser = a.CreatedUser.UserName,
                });
                _logger.LogInformation($"Successfully fetched {auctionViewModels.Count()} auctions.");
                return auctionViewModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching auctions.");
                return Enumerable.Empty<AuctionViewModel>();
            }
        }

        public async Task<AuctionViewModel> GetAuctionDetailsAsync(int id)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(id);
            if (auction == null)
            {
                _logger.LogWarning("Auction with ID {AuctionId} not found.", id);
                throw new InvalidOperationException("The Auction is not found.");
            }
            _logger.LogInformation("Fetched details for auction ID {AuctionId}.", id);
            var highestBid = await _bidRepository.GetHighestBidForAuctionAsync(id);
            return new AuctionViewModel
            {
                Id = auction.Id,
                Title = auction.Title,
                Description = auction.Description,
                StartingPrice = auction.StartingPrice,
                CurrentPrice = highestBid?.Amount ?? 0,
                EndDate = auction.EndDate,
                UserId = auction.UserCreatedId,
                CreatedUser = auction.CreatedUser.UserName,
                HighestBidder = highestBid?.User.UserName ?? "",
            };
        }

        public async Task PlaceBidAsync(BidViewModel bidModel, string userId)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(bidModel.AuctionId);
            if (auction == null || auction.IsClosed)
            {
                _logger.LogWarning("Auction with ID {AuctionId} not found.", bidModel.AuctionId);
                throw new InvalidOperationException("Auction not found or already closed.");
            }
            if (bidModel.Amount <= auction.CurrentPrice)
            {
                throw new Exception("Bid amount must be higher than the current price.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.WalletBalance < bidModel.Amount)
            {
                throw new Exception("Insufficient balance to place this bid.");
            }
            var bid = new Bid
            {
                AuctionId = bidModel.AuctionId,
                UserId = userId,
                Amount = bidModel.Amount,
            };
            try
            {
                await _bidRepository.AddBidAsync(bid);
                await _auctionRepository.UpdateAuctionCurrentPriceAsync(bidModel.AuctionId, bidModel.Amount);
                _logger.LogInformation($"Bid placed by {userId} for auction {auction.Id}, new price: {bidModel.Amount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while placing a bid for auction ID {AuctionId}.", bidModel.AuctionId);
                throw new Exception("An error occurred while placing your bid. Please try again later.");
            }
        }
        public async Task AddAuctionAsync(AuctionViewModel auctionViewModel, string userId)
        {
            // Manually map AuctionViewModel to Auction entity
            var auction = new AuctionItem
            {
                Title = auctionViewModel.Title,
                Description = auctionViewModel.Description,
                StartingPrice = auctionViewModel.StartingPrice,
                CurrentPrice = auctionViewModel.StartingPrice,
                EndDate = auctionViewModel.EndDate,
                IsClosed = false,
                UserCreatedId = userId
            };

            await _auctionRepository.AddAuctionAsync(auction);
            _logger.LogInformation("New auction with ID {AuctionId} created by user ID {UserId}.", auction.Id, userId);
        }

    }
}
