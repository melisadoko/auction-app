
using Auction.Web.IServices;
using Auction.Web.Services;

namespace Auction.Web.BackgroundService
{
    public class AuctionEndService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        private readonly ILogger<AuctionEndService> _logger;

        public AuctionEndService(IServiceProvider serviceProvider, ILogger<AuctionEndService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(HandleAuctions, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }
        private async void HandleAuctions(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var auctionService = scope.ServiceProvider.GetRequiredService<IAuctionService>();
                _logger.LogInformation("Checking for ended auctions at: {time}", DateTime.Now);
                await auctionService.CheckEndAuctionsAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
