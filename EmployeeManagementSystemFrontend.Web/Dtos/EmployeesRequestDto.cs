namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class EmployeesRequestDto
    {
        public int DepartmentId{get;set;}

        public int PageNumber{get;set;}=1;

        public int PageSize{get;set;}=5;

        public string SearchTerm{get;set;}=string.Empty;

        public string SortBy{get;set;}="id";

        public string SortOrder{get;set;}="asc";
    }
}