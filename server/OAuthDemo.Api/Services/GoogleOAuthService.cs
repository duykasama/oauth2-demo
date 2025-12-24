using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using OAuthDemo.Api.Models;

namespace OAuthDemo.Api.Services;

public interface IGoogleOAuthService
{

    Task<AppUser> LogInWihGoogleAsync(GoogleLoginRequest request);
}


internal class GoogleOAuthService : IGoogleOAuthService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _redirectUri;
    private readonly string _getTokenUrl;


    public GoogleOAuthService(IConfiguration configuration)
    {
        var googleOAuthSection = configuration.GetSection("GoogleOAuth");
        var clientId = googleOAuthSection.GetValue<string>("ClientId");
        var clientSecret = googleOAuthSection.GetValue<string>("ClientSecret");
        var redirectUri = googleOAuthSection.GetValue<string>("RedirectUri");
        var getTokenUrl = googleOAuthSection.GetValue<string>("GetTokenUrl");

        ArgumentNullException.ThrowIfNullOrEmpty(clientId, nameof(clientId));
        ArgumentNullException.ThrowIfNullOrEmpty(clientSecret, nameof(clientSecret));
        ArgumentNullException.ThrowIfNullOrEmpty(redirectUri, nameof(redirectUri));
        ArgumentNullException.ThrowIfNullOrEmpty(getTokenUrl, nameof(getTokenUrl));

        _clientId = clientId;
        _clientSecret = clientSecret;
        _redirectUri = redirectUri;
        _getTokenUrl = getTokenUrl;
    }

    public async Task<AppUser> LogInWihGoogleAsync(GoogleLoginRequest request)
    {
        var httpClient = new HttpClient();
        var code = WebUtility.UrlDecode(request.code);
        var requestParams = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", _clientId },
            { "client_secret", _clientSecret },
            { "redirect_uri", _redirectUri },
            { "grant_type", "authorization_code" }
        };

        var content = new FormUrlEncodedContent(requestParams);
        var response = await httpClient.PostAsync(_getTokenUrl, content);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Token is invalid");

        var authObject = JsonSerializer.Deserialize<Response>(await response.Content.ReadAsStringAsync());
        if (authObject?.IdToken == null)
            throw new Exception("Id token cannot be null");

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.ReadJwtToken(authObject.IdToken);
        securityToken.Claims.TryGetValue("email", out var email);
        securityToken.Claims.TryGetValue("name", out var name);
        securityToken.Claims.TryGetValue("picture", out var picture);

        return new AppUser(email, name, picture);
    }


    internal class Response
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("id_token")]
        public string? IdToken { get; set; }
    }
}

public static class Extensions
{

    public static void TryGetValue(this IEnumerable<Claim> claims, string claimType, out string value)
    {
        value = claims.FirstOrDefault(c => c.Type == claimType)?.Value ?? string.Empty;
    }
}
