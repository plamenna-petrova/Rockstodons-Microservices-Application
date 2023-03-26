using Catalog.API.Services.Data.Interfaces;

namespace Catalog.API.Utils
{
    public class JWTRefreshTokenCache : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IIdentityService _identityService;

        public JWTRefreshTokenCache(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _identityService.RemoveExpiredRefreshTokens(DateTime.Now);
        }

        public Task StopAsync(CancellationToken stoppingToken)
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
