using Microsoft.EntityFrameworkCore;
using TextShare.API.Contexts;

namespace TextShare.API.BackgroundServices;


/// <summary>
/// Deletes expired texts every minute
/// </summary>
public class ExpiredFileCleanUpService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ExpiredFileCleanUpService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TextShareContext>();

            await context
                .SharedTexts
                .Where(x => x.ExpirationDate < DateTime.UtcNow)
                .ExecuteDeleteAsync(stoppingToken);

            await Task.Delay(60_000, stoppingToken);
        }
    }
}