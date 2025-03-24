using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ChildGrowth.AdminPage.Authentication;

public class AuthorizedHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public AuthorizedHttpClient(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    private async Task SetAuthorizationHeader()
    {
        // Get token from local storage
        var token = await _localStorage.GetItemAsync<string>("authToken");
        
        // Clear previous Authorization header if it exists
        _httpClient.DefaultRequestHeaders.Authorization = null;
        
        // Set the Authorization header if token exists
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<HttpResponseMessage> GetAsync(string requestUri)
    {
        await SetAuthorizationHeader();
        return await _httpClient.GetAsync(requestUri);
    }

    public async Task<T> GetFromJsonAsync<T>(string requestUri)
    {
        await SetAuthorizationHeader();
        return await _httpClient.GetFromJsonAsync<T>(requestUri);
    }

    public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
    {
        await SetAuthorizationHeader();
        return await _httpClient.PostAsync(requestUri, content);
    }

    public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T value)
    {
        await SetAuthorizationHeader();
        return await _httpClient.PostAsJsonAsync(requestUri, value);
    }

    public async Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
    {
        await SetAuthorizationHeader();
        return await _httpClient.PutAsync(requestUri, content);
    }

    public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string requestUri, T value)
    {
        await SetAuthorizationHeader();
        return await _httpClient.PutAsJsonAsync(requestUri, value);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
    {
        await SetAuthorizationHeader();
        return await _httpClient.DeleteAsync(requestUri);
    }

    // Generic method for sending any request with authorization
    public async Task<HttpResponseMessage> SendAuthorizedRequestAsync(HttpRequestMessage request)
    {
        await SetAuthorizationHeader();
        return await _httpClient.SendAsync(request);
    }
}

public class PagingBaseResponse<T>
{
    public int Size { get; set; }
    public int Page { get; set; }
    public int Total { get; set; }
    public int TotalPages { get; set; }
    public IEnumerable<T> Items { get; set; }
}
public class PagingBaseRequest
{
    public int Page { get; set; }
    public int Size { get; set; }
}