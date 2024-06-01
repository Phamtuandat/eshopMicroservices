using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Areas.Identity.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
