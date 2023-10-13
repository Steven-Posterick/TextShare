using Microsoft.EntityFrameworkCore;
using TextShare.API.Contexts;

namespace TextShare.API.BackgroundServices;


/// <summary>
/// Deletes expired texts every minute
/// </summary>
public class ExpiredFileCleanUpService : BackgroundService
{
    private readonly TextShareContext _context;

    public ExpiredFileCleanUpService(TextShareContext context)
    {
        _context = context;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _context
                .SharedTexts
                .Where(x => x.ExpirationDate < DateTime.UtcNow)
                .ExecuteDeleteAsync(stoppingToken);

            await Task.Delay(60_000, stoppingToken);
        }
    }
}