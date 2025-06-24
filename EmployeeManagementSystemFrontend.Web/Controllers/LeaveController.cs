using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemFrontend.Web.Controllers
{
    public class LeaveController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}