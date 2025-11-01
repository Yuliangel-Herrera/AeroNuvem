using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EFAereoNuvem.Services;
public class AuthService
{
    private readonly HttpClient _httpClient;
    private string? _token;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var body = JsonSerializer.Serialize(new { email, password });
        var content = new StringContent(body, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://localhost:7157/v1/accounts/login", content);

        if (!response.IsSuccessStatusCode)
            return false;

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        _token = doc.RootElement.GetProperty("data").GetProperty("token").GetString();

        // Define o header Authorization globalmente
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

        return true;
    }

    public void Logout()
    {
        _token = null;
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public bool IsAuthenticated() => !string.IsNullOrEmpty(_token);
}

