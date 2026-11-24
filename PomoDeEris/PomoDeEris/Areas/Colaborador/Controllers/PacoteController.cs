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
                return RedirectToAction("Index", "Pacote");
            }
            else
            {
                return View(pacote);
            }


        }
        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            Pacote pacote = _pacoteRepository.ObterPacote(id);
            return View(pacote);
        }

        [HttpPost]
        public IActionResult Atualizar(Pacote pacote, List<int> servicosIds, IFormFile arquivo)
        {
            ModelState.Remove("Imagem");
            ModelState.Remove("Servicos");
            ModelState.Remove("arquivo");
            ModelState.Remove("servicosIds");
            foreach (var chave in ModelState.Keys.Where(k => k.StartsWith("Servicos[")).ToList())
                ModelState.Remove(chave);

            pacote.Servicos = (servicosIds ?? new List<int>()) // verifica se ta nulo se n cria uma lista
                .Where(id => id > 0) // id > 0 (valido)
                .Distinct() // distito
                .Select(id => new Servico { IdServico = id }) // transforma em servico
                .ToList();

            if (ModelState.IsValid)
            {
                if (arquivo != null && arquivo.Length > 0)
                {
                    // salve o arquivo do mesmo jeito que você faz no Cadastrar
                    pacote.Imagem = GerenciaArquivos.CadastrarImagemProduto(arquivo);
                }
                // sem arquivo novo, Imagem fica vazia e a procedure mantém a atual

                _pacoteRepository.Atualizar(pacote);
                return RedirectToAction("Index");
            }

            return View(pacote);
        }
    }
}
