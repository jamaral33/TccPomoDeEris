using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Filtro;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacaoAttribute]
    public class PacoteController : Controller
    {
        private IPacoteRepository _pacoteRepository;
        private IServicoRepository _servicoRepository;
        public PacoteController(IPacoteRepository pacoteRepository, IServicoRepository servicoRepository)
        {
            _pacoteRepository = pacoteRepository;
            _servicoRepository = servicoRepository;
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
        }
    }
}
