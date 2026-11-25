using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Criptografia;
using PomoDeEris.Libraries.Login;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;
        private IClienteRepository _clienteRepository;
        private LoginCliente _loginCliente;
        private CriptografiaSenha criptografia = new CriptografiaSenha();
        private IAtendimentoRepository _atendimentoRepository;


        public ClienteController(ILogger<ClienteController> logger, IClienteRepository clienteRepository, LoginCliente loginCliente, IAtendimentoRepository atendimentoRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
            _atendimentoRepository = atendimentoRepository;
        }


        public IActionResult Index()
        {
            var cliente = _loginCliente.GetCliente();
            if(cliente == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View(cliente);
            }
        }

        [HttpPost]
        public IActionResult Atualizar([FromForm] Cliente cliente)
        {
            if (!ModelState.IsValid) 
            {
                return View("Index", cliente);
            }
            if(cliente.Senha != null)
            {
                cliente.Senha = criptografia.hasharSenha(cliente.Senha);
            }

            cliente.Situacao = "Ativo";

           _clienteRepository.Atualizar(cliente);
            _loginCliente.Atualizar(cliente);


            return RedirectToAction("Index");
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Historico(string? filtro)
        {

            var cliente = _loginCliente.GetCliente();

            if (cliente == null)
            {
                return RedirectToAction("Login");
            }
            var atendimentosCliente = _atendimentoRepository.ObterTodosAtendimentosCliente(cliente.Id);

            ViewBag.QtdConsultas = atendimentosCliente.Where(a => a.Situacao != "Cancelado").ToList().Count();
            ViewBag.ProximaConsulta = atendimentosCliente.Where(a => a.DataHora > DateTime.Now && a.Situacao =="Agendado").OrderBy(a => a.DataHora).Select(a => a.DataHora).FirstOrDefault();
            ViewBag.QuantiaGasta = atendimentosCliente.Where(a => a.Situacao == "Concluido" || a.Situacao == "Agendado").Sum(a => a.Pagamento.Valor);


            var atendimentosFiltrado = atendimentosCliente = atendimentosCliente.Where(a => string.IsNullOrEmpty(filtro) || a.Situacao == filtro).ToList(); //Se o filtro estiver vazio ele retorna tudo só com um valor ele filtra


            ViewBag.Atendimentos = atendimentosFiltrado;
            ViewBag.FiltroSelecionado = filtro;
            return View(cliente);
            
        }
        [HttpPost]
        public IActionResult Logout()
        {
            _loginCliente.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult DesativarConta()
        {
            var cliente = _loginCliente.GetCliente();

            if (cliente != null)
            {
                cliente.Situacao = "Inativo";
                _clienteRepository.Atualizar(cliente);
                _loginCliente.Logout();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginClienteViewModel cliente)
        {
            Cliente clienteDB = _clienteRepository.Login(cliente.Email, cliente.Senha);
            if (clienteDB != null && clienteDB.Email != null && clienteDB.Senha != null)
            {
                _loginCliente.Login(clienteDB);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //ERRO NA SESSAO
                ViewData["MSG_E"] = "Usuário não localizado, por favor verifique o email e senha digitado";
                return View();
            }
        }
        public IActionResult Cadastro()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastro([FromForm] Cliente cliente)
        {
            if (!ModelState.IsValid) //Valida a model
            {
                return View(cliente);
            }

            _clienteRepository.Cadastrar(cliente);

            Cliente clienteDB = _clienteRepository.Login(cliente.Email, cliente.Senha);

            if (clienteDB != null && clienteDB.Email != null && clienteDB.Senha != null)
            {
                _loginCliente.Login(clienteDB);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult RedefinirSenhaEmail()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RedefinirSenhaCodigo(string email)
        {
            var cliente = _clienteRepository
            return View();
        }
        [HttpGet]
        public IActionResult RedefinirSenha(Cliente cliente)
        {
        }
    }
}
