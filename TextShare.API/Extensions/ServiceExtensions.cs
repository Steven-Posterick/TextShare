using Microsoft.EntityFrameworkCore;
using TextShare.API.BackgroundServices;
using TextShare.API.Contexts;
using TextShare.API.Services;

namespace TextShare.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        // Contexts
        services.AddDbContextPool<TextShareContext>((provider, options) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("TextShareConnectionString");
            options.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
        });

        // Services
        services.AddScoped<ITextShareService, TextShareService>();
        services.AddScoped<IEncryptionService, EncryptionService>();
        
        // Background Services
        services.AddHostedService<ExpiredFileCleanUpService>();
    }
}