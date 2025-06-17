namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class UserRegistrationDto
    {
        public string FirstName { get; set; }=string.Empty;

        public string LastName { get; set; } =string.Empty;

        public string UserName { get; set; } =string.Empty;

        public string Email { get; set; } =string.Empty;

        public string Address { get; set; }=string.Empty;

        public string Zipcode { get; set; }=string.Empty;

        public string MobileNo { get; set; }=string.Empty;

        public string Password { get; set; }=string.Empty;

        public int CountryId { get; set; }

        public int StateId { get; set; }

        public int CityId { get; set; }

        public int RoleId { get; set; }
    }
}