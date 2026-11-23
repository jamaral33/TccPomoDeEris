using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Filtro;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;
using PomoDeEris.Libraries.Arquivos;

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
            
            return View(_pacoteRepository.ObterTodosPacotes());
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Pacote pacote, List<int> ServicosIds, IFormFile arquivo)
        {
            var Caminho = GerenciaArquivos.CadastrarImagemProduto(arquivo);
            pacote.Imagem = Caminho;

            ModelState.Remove(nameof(Pacote.Imagem));
            ModelState.Remove(nameof(Pacote.Servicos));
            ModelState.Remove(nameof(ServicosIds));

            pacote.Servicos = new List<Servico>();
            foreach (var id in ServicosIds)
            {
                Servico servico = new Servico()
                {
                    IdServico = id
                };
                pacote.Servicos.Add(servico);
            }

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
