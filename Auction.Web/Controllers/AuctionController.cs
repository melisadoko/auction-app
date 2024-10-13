using Auction.Web.IServices;
using Auction.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Auction.Web.Controllers
{
    [Authorize]
    public class AuctionController : Controller
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }
        public async Task<IActionResult> Index()
        {
            var auctions = await _auctionService.GetAllAuctionsAsync();
            return View(auctions);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _auctionService.DeleteAuctionAsync(id, userId);
                TempData["SuccessMessage"] = "Auction deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuctionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                await _auctionService.AddAuctionAsync(model, userId);
                TempData["SuccessMessage"] = "Auction created successfully.";
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var auction = await _auctionService.GetAuctionDetailsAsync(id);
                if (auction == null)
                {
                    return NotFound();
                }

                var viewModel = new AuctionDetailsViewModel
                {
                    Auction = auction,  
                    Bid = new BidViewModel { AuctionId = id }  
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBid(AuctionDetailsViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Detail", viewModel);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                await _auctionService.PlaceBidAsync(viewModel.Bid, userId);
                TempData["SuccessMessage"] = "Bid placed successfully.";
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Detail", viewModel);
            }
            return RedirectToAction("Index");
        }
    }
}
