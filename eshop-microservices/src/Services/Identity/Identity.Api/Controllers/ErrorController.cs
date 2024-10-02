using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        [Route("Error/500")]
        public IActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View();
        }


        [Route("Error/403")]
        public IActionResult Fobbiden()
        {
            Response.StatusCode = 403;
            return View();
        }

    }


}
