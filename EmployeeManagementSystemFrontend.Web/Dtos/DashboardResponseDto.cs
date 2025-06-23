namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class DashboardResponseDto
    {
        public Dictionary<string,int> DepartmentWiseEmployees{get;set;}=[];

        public Dictionary<string,int> GenderWiseEmployees{get;set;}=[];

        public Dictionary<int,int> AgeWiseEmployees{get;set;}=[];

        public Dictionary<string,int> TimeWiseEmployees{get;set;}=[];

        public Dictionary<string,int> CountryWiseEmployees{get;set;}=[];
    }
}