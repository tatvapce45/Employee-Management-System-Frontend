using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemFrontend.Web.Controllers
{
    public class AttendanceController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}