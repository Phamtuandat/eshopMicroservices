using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Areas.Identity.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
