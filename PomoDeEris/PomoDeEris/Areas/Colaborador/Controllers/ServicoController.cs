using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Filtro;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacaoAttribute]
    public class ServicoController : Controller
    {
        private IServicoRepository _servicoRepository;
        public ServicoController(IServicoRepository servicoRepository)
        {
            _servicoRepository = servicoRepository;
        }
        public IActionResult Index()
        {
            return View(_servicoRepository.ObterTodosServicos());
                
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Servico servico)
        {
            if (ModelState.IsValid)
            {
                _servicoRepository.Cadastrar(servico);
                return RedirectToAction("Index", "Servico");

            }
            else
            {
                return View(servico);
            }
        }
    }
}
