namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class TokenRefreshResponseDto
    {
        public required string RefreshToken { get; set; }

        public required string AccessToken { get; set; }
    }
}