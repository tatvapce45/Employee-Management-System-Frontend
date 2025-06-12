using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class TokensDto
    {   
        public string AccessToken{get;set;}=string.Empty;

        public string RefreshToken{get;set;}=string.Empty;
    }
}