namespace EmployeeManagementSystemFrontend.Web.Dtos
{
    public class StatesDto
    {
        public int Id { get; set; }

        public string Name { get; set; }=string.Empty;
    }

    public class StatesResponseDto
    {
        public List<StatesDto> StatesDtos {get;set;}=[];
    }
}