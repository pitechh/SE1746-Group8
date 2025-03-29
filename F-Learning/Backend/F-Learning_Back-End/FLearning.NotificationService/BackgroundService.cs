using FLearning.NotificationService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FLearning.NotificationService
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<NotificationBackgroundService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public NotificationBackgroundService(
            IServiceProvider services,
            ILogger<NotificationBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Processing pending notifications...");

                try
                {
                    using (var scope = _services.CreateScope())
                    {
                        var notificationService =
                            scope.ServiceProvider.GetRequiredService<INotificationService>();

                        await notificationService.ProcessPendingNotificationsAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing pending notifications");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("Notification Background Service is stopping.");
        }
    }
}
