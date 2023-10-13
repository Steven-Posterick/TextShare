using Microsoft.EntityFrameworkCore;
using TextShare.API.Contexts;
using TextShare.API.Exceptions;
using TextShare.API.Models.Entities;
using TextShare.API.Models.Requests;
using TextShare.API.Models.Responses;

namespace TextShare.API.Services;

public interface ITextShareService
{
    Task<SharedText> CreateTextAsync(TextCreateRequest textRequest);
    Task<TextReadResponse> GetTextAsync(Guid id, string? password);
    Task<TextDetailResponse> GetTextDetailsAsync(Guid id);
}

public class TextShareService : ITextShareService
{
    private readonly TextShareContext _context;
    private readonly IEncryptionService _encryptionService;
    private readonly DbSet<SharedText> _dbSet;

    public TextShareService(TextShareContext context, IEncryptionService encryptionService)
    {
        _context = context;
        _dbSet = context.Set<SharedText>();
        _encryptionService = encryptionService;
    }
    
    public async Task<SharedText> CreateTextAsync(TextCreateRequest textRequest)
    {
        string? passwordHash = null;
        if (!string.IsNullOrEmpty(textRequest.Password))
        {
            passwordHash = _encryptionService.HashPassword(textRequest.Password);
        }

        var sharedText = new SharedText
        {
            Content = textRequest.Content,
            CreationDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddMonths(1),
            BurnAfterReading = textRequest.BurnAfterReading,
            PasswordHash = passwordHash
        };

        _dbSet.Add(sharedText);
        await _context.SaveChangesAsync();
        
        return sharedText;
    }

    public async Task<TextReadResponse> GetTextAsync(Guid id, string? password)
    {
        var sharedText = await _dbSet.FindAsync(id);

        if (sharedText == null)
            throw new EntityNotFoundException();

        if (sharedText.ExpirationDate <= DateTime.UtcNow)
        {
            await RemoveTextAsync(sharedText);
            throw new EntityNotFoundException();
        }

        if (!string.IsNullOrEmpty(sharedText.PasswordHash) && !_encryptionService.VerifyPassword(password, sharedText.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid password.");
        }

        var readResponse = new TextReadResponse
        {
            Id = sharedText.Id,
            Content = sharedText.Content
        };

        if (sharedText.BurnAfterReading)
        {
            await RemoveTextAsync(sharedText);
        }

        return readResponse;

    }

    public async Task<TextDetailResponse> GetTextDetailsAsync(Guid id)
    {
        var sharedText = await _dbSet.FindAsync(id);

        if (sharedText == null)
            throw new EntityNotFoundException();

        return new TextDetailResponse
        {
            Id = sharedText.Id,
            BurnAfterRead = sharedText.BurnAfterReading,
            IsPasswordProtected = sharedText.PasswordHash != null
        };
    }

    private async Task RemoveTextAsync(SharedText sharedText)
    {
        _dbSet.Remove(sharedText);
        await _context.SaveChangesAsync();
    }
}