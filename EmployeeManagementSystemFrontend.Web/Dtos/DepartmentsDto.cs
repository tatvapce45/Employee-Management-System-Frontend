using EmployeeManagementSystemFrontend.Web.Models;

namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class DepartmentsDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<Department> Departments { get; set; }=[];
    }

    public class CreateDepartmentDto
    {
        public required string Name { get; set; }

        public required string Description { get; set; }
    }

    public class UpdateDepartmentDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }
    }
}