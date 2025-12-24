using Microsoft.AspNetCore.Mvc;
using OAuthDemo.Api.Models;
using OAuthDemo.Api.Services;

namespace OAuthDemo.Api.Controllers;

[Route("/api/[controller]")]
[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IGoogleOAuthService _googleOAuthService;

    [Route("oauth/google")]
    [HttpGet]
    public IActionResult GetGoogleOauthUrl()
    {
        var googleOAuthSection = _configuration.GetSection("GoogleOAuth");
        var clientId = googleOAuthSection.GetValue(typeof(string), "ClientId");
        var redirectUri = googleOAuthSection.GetValue(typeof(string), "RedirectUri");
        var finalUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&scope=email%20profile%20openid&redirect_uri={redirectUri}&response_type=code";

        return Ok(new { Url = finalUrl });
    }

    [Route("oauth/google")]
    [HttpPost]
    public async Task<IActionResult> LogInWithGoogle([FromBody] GoogleLoginRequest request)
    {
        var user = await _googleOAuthService.LogInWihGoogleAsync(request);

        return Ok(user);
    }

    public AuthController(IConfiguration configuration, IGoogleOAuthService googleOAuthService)
    {
        _configuration = configuration;
        _googleOAuthService = googleOAuthService;
    }
}
