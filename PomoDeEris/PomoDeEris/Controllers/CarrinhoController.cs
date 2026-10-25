using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Carrinho;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Controllers
{
    public class CarrinhoController : Controller
    {
        private IPacoteRepository _pacoteRepository;
        private CarrinhoCookie _carrinhoCookie;

        public CarrinhoController(IPacoteRepository pacoteRepository, CarrinhoCookie carrinhoCookie)
        {
            _pacoteRepository = pacoteRepository;
            _carrinhoCookie = carrinhoCookie;
        }
        public IActionResult Index()
        {
            return View(_carrinhoCookie.Consultar());
        }

        [HttpPost]
        public IActionResult AdicionarItem(int id)
        {
            Pacote pacote = _pacoteRepository.ObterPacote(id);

            if (pacote == null)
            {
                ViewData["MSG_CARRINHO"] = "Erro produto não localizado";
            }
            else
            {
                var item = new Pacote()
                {
                    IdPacote = id,
                    Quantidade = 1,
                    Nome = pacote.Nome,
                    Descricao = pacote.Descricao,
                    Valor = pacote.Valor,
                    Imagem = pacote.Imagem,
                };
                _carrinhoCookie.Cadastrar(item);

                return RedirectToAction(nameof(Index));

            }
            return View();
        }
        [HttpPost]
        public IActionResult RemoverPacote(int id)
        {
            Pacote pacote = _pacoteRepository.ObterPacote(id);

            _carrinhoCookie.Remover(pacote);

            return RedirectToAction(nameof(Index));

        }
    }
}
