namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class CitiesDto
    {
        public int Id { get; set; }

        public string Name { get; set; }=string.Empty;
    }

    public class CitiesResponseDto
    {
        public List<CitiesDto> CitiesDtos {get;set;}=[];
    }
}