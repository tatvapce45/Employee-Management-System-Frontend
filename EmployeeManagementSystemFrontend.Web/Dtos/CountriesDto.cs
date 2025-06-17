namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class CountriesDto
    {
        public int Id { get; set; }

        public string Name { get; set; }=string.Empty;
    }

    public class CountriesResponseDto
    {
        public List<CountriesDto> CountriesDtos {get;set;}=[];
    }
}