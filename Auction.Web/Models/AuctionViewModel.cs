using System.ComponentModel.DataAnnotations;
using Auction.Web.Models.Validation;

namespace Auction.Web.Models
{
    public class AuctionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Product name must be greater than 3.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(1000, MinimumLength = 11, ErrorMessage = "Product name must be greater than 10.")]
        public string Description { get; set; }

        [Display(Name = "Starting Bid")]
        [Required(ErrorMessage = "Starting price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Starting price must be greater than zero.")]
        public decimal StartingPrice { get; set; }
        public decimal? CurrentPrice { get; set; }

        public string? HighestBidder { get; set; }

        [Display(Name = "End Date")]
        [Required(ErrorMessage = "End date is required.")]
        [FutureDate(ErrorMessage = "End date must be in the future.")]
        public DateTime EndDate { get; set; }
        public string? UserId { get; set; }
        [Display(Name = "Seller")]
        public string? CreatedUser { get; set; }

    }
}
