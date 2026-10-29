using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Models;

namespace PomoDeEris.Controllers
{
    public class AtendimentoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmarHorario()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Agendar()
        {
            Atendimento atendimento = new Atendimento()
            {
            }

            return View();
        }

        [HttpGet]
        public IActionResult Detalhes(int id)
        {
            return View();
        }
    }
}
