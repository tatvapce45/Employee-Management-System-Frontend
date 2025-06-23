using System.Net.Http.Headers;

namespace EmployeeManagementSystemFrontend.Web.Common
{
    public class JwtTokenHandler(TokensHelper tokensHelper) : DelegatingHandler
    {
        private readonly TokensHelper _tokensHelper = tokensHelper;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokensHelper.GetAccessToken();

            if (!string.IsNullOrEmpty(token))
            {
                var isValid = await _tokensHelper.ValidateToken(token);
                if (!isValid)
                {
                    token = await _tokensHelper.RefreshAccessToken();
                }

                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
