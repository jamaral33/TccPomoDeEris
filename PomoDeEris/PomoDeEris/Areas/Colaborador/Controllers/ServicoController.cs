using Microsoft.AspNetCore.Mvc;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class ServicoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }
    }
}
