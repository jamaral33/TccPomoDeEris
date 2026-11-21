using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    public class PacoteController : Controller
    {
        private IPacoteRepository _pacoteRepository;
        public PacoteController(IPacoteRepository pacoteRepository)
        {
            _pacoteRepository = pacoteRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Pacote pacote)
        {
            if (ModelState.IsValid)
            {
                _pacoteRepository.Cadastrar(pacote);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(pacote);
            }
        }])
    }
}
