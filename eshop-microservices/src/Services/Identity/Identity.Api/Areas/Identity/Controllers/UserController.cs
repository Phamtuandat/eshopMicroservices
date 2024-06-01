using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Areas.Identity.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
