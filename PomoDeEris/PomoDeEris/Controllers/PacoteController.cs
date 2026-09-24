using Microsoft.AspNetCore.Mvc;

namespace PomoDeEris.Controllers
{
    public class PacoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
