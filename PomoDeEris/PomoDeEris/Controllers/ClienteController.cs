using Microsoft.AspNetCore.Mvc;

namespace PomoDeEris.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
