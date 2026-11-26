using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Controllers
{
    public class PacoteController : Controller
    {
        private IPacoteRepository _pacoteRepository;
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
        public IActionResult Promocoes()
        {
            var pacotes = _pacoteRepository.ObterTodosPacotes().Where(p => p.Desconto > 0).ToList();
            return View(pacotes);
        }

        public IActionResult Detalhes(int id)
        {
            Pacote pacote = _pacoteRepository.ObterPacote(id);
            return View(pacote);
        }
    }
}
