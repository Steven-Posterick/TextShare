using TextShare.Common.Models.Requests;
using TextShare.Common.Models.Responses;

namespace TextShare.Web.Services;

public interface ITextShareService
{
    Task<TextDetailResponse?> CreateTextAsync(TextCreateRequest request);
    Task<TextDetailResponse?> GetTextDetailsAsync(Guid id);
    Task<TextReadResponse?> GetTextAsync(Guid id, string? password = null);
}

public class TextShareApiService : BaseApiService, ITextShareService
{
    public TextShareApiService(HttpClient http) : base(http) {}

    private const string ServiceName = "TextShare";

    public async Task<TextDetailResponse?> CreateTextAsync(TextCreateRequest request)
    {
        return await PostAsync<TextDetailResponse>(ServiceName, request);
    }

    public async Task<TextDetailResponse?> GetTextDetailsAsync(Guid id)
    {
        return await GetAsync<TextDetailResponse>($"{ServiceName}/details/{id}");
    }

    public async Task<TextReadResponse?> GetTextAsync(Guid id, string? password = null)
    {
        var textReadRequest = new TextReadRequest { Password = password };
        return await PostAsync<TextReadResponse>($"{ServiceName}/{id}", textReadRequest);
    }
}