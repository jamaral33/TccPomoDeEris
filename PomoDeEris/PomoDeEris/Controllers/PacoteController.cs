using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Controllers
{
    public class PacoteController : Controller
    {
        IPacoteRepository _pacoteRepository;
        public PacoteController(IPacoteRepository pacoteRepository)
        {
            _pacoteRepository = pacoteRepository;
        }

        public IActionResult Index(string? pesquisa)
        {
            if(string.IsNullOrEmpty(pesquisa))
            {
                pesquisa = "";
            }
            var pacotes = _pacoteRepository.PesquisarPacotes(pesquisa);

            return View(pacotes);
        }
    }
}
