using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;

namespace EmployeeManagementSystemFrontend.Web.Common
{
    public class JwtAuthorizeFilter : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokensHelper _tokensHelper;

        public JwtAuthorizeFilter(IHttpContextAccessor httpContextAccessor, TokensHelper tokensHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokensHelper = tokensHelper;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var endpointMetadata = context.ActionDescriptor.EndpointMetadata;
            if (endpointMetadata.Any(m => m is AllowAnonymousAttribute))
            {
                await next();
                return;
            }

            var httpContext = _httpContextAccessor.HttpContext!;
            var token = _tokensHelper.GetAccessToken();

            // if (string.IsNullOrEmpty(token))
            // {
            //     context.Result = new RedirectToActionResult("Login", "Authentication", null);
            //     return;
            // }

            var isValid = await _tokensHelper.ValidateToken(token ?? "");
            if (!isValid)
            {
                var newToken = await _tokensHelper.RefreshAccessToken();
                if (string.IsNullOrEmpty(newToken))
                {
                    _tokensHelper.ClearTokens();
                    context.Result = new RedirectToActionResult("Login", "Authentication", null);
                    return;
                }
                token = newToken;
            }

            await next();
        }
    }

}
