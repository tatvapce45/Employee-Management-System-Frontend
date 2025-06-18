using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmployeeManagementSystemFrontend.Web.Common
{
    public class JwtAuthorizeFilter : IAsyncActionFilter
    {
        private readonly TokensHelper _tokensHelper;

        public JwtAuthorizeFilter(TokensHelper tokensHelper)
        {
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

            var token = _tokensHelper.GetAccessToken();

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
