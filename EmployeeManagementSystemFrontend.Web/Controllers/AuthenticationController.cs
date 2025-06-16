using EmployeeManagementSystemFrontend.Web.Common;
using EmployeeManagementSystemFrontend.Web.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace EmployeeManagementSystemFrontend.Web.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        private readonly HttpClient _httpClient;

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("EmployeeManagementApi");
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto dto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/authentication/Register", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            var error = await response.Content.ReadAsStringAsync();
            ViewBag.Error = error;
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }


        public async Task<IActionResult> Login()
        {
            string? token = HttpContext.Session.GetString("AccessToken") ?? Request.Cookies["AccessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                // Add the Authorization header with Bearer token
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, "api/authentication/Validate-Token");
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Log the error message for debugging
                    ViewBag.Error = $"Error: {response.StatusCode}. Please check your authentication.";
                    return View();
                }
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(loginDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://localhost:5287/api/authentication/Login", content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<UserLoginDto>>(responseContent);

            if (response.IsSuccessStatusCode && apiResponse != null && apiResponse.Success)
            {
                
                HttpContext.Session.SetString("RememberMe", loginDto.RememberMe.ToString());
                return RedirectToAction("VerifyOtp", "Authentication", new { email = loginDto.Email });
            }

            ViewBag.Error = apiResponse?.Message ?? "Login failed.";
            ViewBag.Errors = apiResponse?.ValidationErrors;
            return View();
        }

        [HttpGet]
        public IActionResult VerifyOtp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string email, string otp)
        {
            var response = await _httpClient.PostAsync($"http://localhost:5287/api/authentication/Verify-OTP?email={email}&otp={otp}", null);
            var content = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<TokensDto>>(content);

            if (response.IsSuccessStatusCode && apiResponse?.Success == true)
            {
                string accessToken = apiResponse.Data!.AccessToken;
                string refreshToken = apiResponse.Data!.RefreshToken;

                _ = bool.TryParse(HttpContext.Session.GetString("RememberMe"), out bool rememberMe);

                if (rememberMe)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.UtcNow.AddDays(30),
                        SameSite = SameSiteMode.Strict
                    };
                    Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
                    Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
                }
                else
                {
                    HttpContext.Session.SetString("AccessToken", accessToken);
                    HttpContext.Session.SetString("RefreshToken", refreshToken);
                }

                HttpContext.Session.Remove("RememberMe");
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = apiResponse?.Message;
            ViewBag.Errors = apiResponse?.ValidationErrors;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRefreshRequestDto refreshRequestDto)
        {
            var content = new StringContent(JsonConvert.SerializeObject(refreshRequestDto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/authentication/Refresh-Token", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<dynamic>(result);
                string newAccessToken = tokenResponse!.AccessToken;

                HttpContext.Session.SetString("AccessToken", newAccessToken);
                return Ok(new { AccessToken = newAccessToken });
            }

            return Unauthorized("Failed to refresh token.");
        }
    }
}
