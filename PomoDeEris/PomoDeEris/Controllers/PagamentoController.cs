using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Models;

namespace PomoDeEris.Controllers
{
    public class PagamentoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Pagar(Atendimento atendimento)
        {
            return View();
        }

    }
}
