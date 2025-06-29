using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class UpdateProfileDto
    {   
        public required int Id { get; set; }

        [Required]
        public required string FirstName { get; set; }

        [Required]
        public required string LastName { get; set; } 

        [Required]
        public required string UserName { get; set; } 

        [Required]
        public required string Email { get; set; } 

        [Required]
        public required string Address { get; set; }

        [Required]
        public required string Zipcode { get; set; }

        [Required]
        public required string MobileNo { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int StateId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public string Gender { get; set; } = null!;

        public int Age { get; set; }

        public decimal Salary { get; set; }

        public int DepartmentId{get;set;}
    }
}