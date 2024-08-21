using Microsoft.AspNetCore.Mvc;

namespace Movie_CRUD.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
