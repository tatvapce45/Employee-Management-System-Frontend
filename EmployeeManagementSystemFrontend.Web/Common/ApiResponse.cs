namespace EmployeeManagementSystemFrontend.Web.Common
{
    public class ApiResponse<T>
{
    public bool Success { get; set; }

    public int StatusCode { get; set; }

    public string? Message { get; set; }

    public T? Data { get; set; }

    public List<string>? ValidationErrors { get; set; }
}
}
