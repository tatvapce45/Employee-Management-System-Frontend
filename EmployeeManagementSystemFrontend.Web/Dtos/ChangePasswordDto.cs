namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class ChangePasswordDto
    {
        public int Id { get; set; }

        public string? CurrentPassword { get; set; }

        public string? NewPassword { get; set; }

        public string? ConfirmNewPassword { get; set; }
    }

}