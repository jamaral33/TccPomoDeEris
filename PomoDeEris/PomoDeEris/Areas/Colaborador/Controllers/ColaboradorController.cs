using Microsoft.AspNetCore.Mvc;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    public class ColaboradorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
