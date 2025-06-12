using System;
using System.Collections.Generic;

namespace EmployeeManagementSystemFrontend.Web.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime HiringDate { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int DepartmentId { get; set; }

    public string Email { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public int Age { get; set; }

    public decimal Salary { get; set; }

    public virtual Department Department { get; set; } = null!;
}
