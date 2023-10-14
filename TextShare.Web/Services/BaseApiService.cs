using System.Net.Http.Json;

namespace TextShare.Web.Services;

public interface IApiService
{
    
}

public class BaseApiService : IApiService
{
    private readonly HttpClient _http;

    protected BaseApiService(HttpClient http)
    {
        _http = http;
    }

    protected async Task<T?> PostAsync<T>(string endpoint, object request)
    {
        var response = await _http.PostAsJsonAsync(endpoint, request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }

    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        var response = await _http.GetAsync(endpoint);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>();
    }
}