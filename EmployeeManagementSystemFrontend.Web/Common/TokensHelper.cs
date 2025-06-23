using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EmployeeManagementSystemFrontend.Web.Common;
using EmployeeManagementSystemFrontend.Web.Dtos;
using Newtonsoft.Json;

public class TokensHelper(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;


    public string? GetAccessToken()
    {
        var context = _httpContextAccessor.HttpContext!;
        return context.Session.GetString("AccessToken") ?? context.Request.Cookies["AccessToken"];
    }

    public string? GetRefreshToken()
    {
        var context = _httpContextAccessor.HttpContext!;
        return context.Session.GetString("RefreshToken") ?? context.Request.Cookies["RefreshToken"];
    }

    public void StoreTokens(string accessToken, string refreshToken, bool rememberMe)
    {
        var context = _httpContextAccessor.HttpContext!;

        if (rememberMe)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                Expires = DateTime.UtcNow.AddDays(30),
                SameSite = SameSiteMode.Strict
            };
            context.Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
            context.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
        }
        else
        {
            context.Session.SetString("AccessToken", accessToken);
            context.Session.SetString("RefreshToken", refreshToken);
        }
    }

    public async Task<bool> ValidateToken(string token)
    {
        var client = _httpClientFactory.CreateClient("TokenApi");
        var request = new HttpRequestMessage(HttpMethod.Get, "api/authentication/Validate-Token");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        return true;
    }

    public async Task<string?> RefreshAccessToken()
    {
        var refreshToken = GetRefreshToken();
        if (string.IsNullOrEmpty(refreshToken))
        {
            return null;
        }

        var client = _httpClientFactory.CreateClient("TokenApi");

        var requestDto = new TokenRefreshRequestDto { RefreshToken = refreshToken };
        var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("api/authentication/Refresh-Token", content);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApiResponse<TokenRefreshResponseDto>>(responseContent);

        if (result?.Success == true && result.Data != null)
        {
            StoreTokens(result.Data.AccessToken, result.Data.RefreshToken, false);
            return result.Data.AccessToken;
        }

        return null;
    }

    public void ClearTokens()
    {
        var context = _httpContextAccessor.HttpContext!;
        context.Session.Remove("AccessToken");
        context.Session.Remove("RefreshToken");
        context.Response.Cookies.Delete("AccessToken");
        context.Response.Cookies.Delete("RefreshToken");
    }

    public IDictionary<string, string>? GetClaimsFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return null;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var claimsDict = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);

        return claimsDict;
    }

}
