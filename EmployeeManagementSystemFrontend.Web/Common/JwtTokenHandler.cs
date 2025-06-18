using System.Net.Http.Headers;

namespace EmployeeManagementSystemFrontend.Web.Common
{
    public class JwtTokenHandler : DelegatingHandler
    {
        private readonly TokensHelper _tokensHelper;

        public JwtTokenHandler(TokensHelper tokensHelper)
        {
            _tokensHelper = tokensHelper;
        }

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
