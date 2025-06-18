using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeeManagementSystemFrontend.Web.Common
{
    public class UserClaimsHelper(IHttpContextAccessor httpContextAccessor)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public IDictionary<string, string>? GetUserClaims()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var session = httpContext?.Session;
            var token = session?.GetString("AccessToken") ??
                        httpContext?.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(token))
                {
                    return null;
                }

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token)) 
            {
                return null;
            }

            var jwt = handler.ReadJwtToken(token);

            return jwt.Claims.ToDictionary(c => c.Type, c => c.Value);
        }

        public string? GetClaim(string key)
        {
            var claims = GetUserClaims();
            return claims != null && claims.ContainsKey(key) ? claims[key] : null;
        }
    }
}
