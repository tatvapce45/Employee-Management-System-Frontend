namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class RolesDto
    {
        public int Id { get; set; }

        public string Name { get; set; }=string.Empty;
    }

    public class RolesResponseDto
    {
        public List<RolesDto> RolesDtos {get;set;}=[];
    }
}