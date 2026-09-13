using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Login;
using PomoDeEris.Models;
using PomoDeEris.Repository;
using PomoDeEris.Repository.Contract;
using System.Diagnostics;

namespace PomoDeEris.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IClienteRepository _clienteRepository;
        private LoginCliente _loginCliente;


        public HomeController(ILogger<HomeController> logger, IClienteRepository clienteRepository, LoginCliente loginCliente)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginClienteViewModel cliente)
        {
            Cliente clienteDB = _clienteRepository.Login(cliente.Email, cliente.Senha);
            if (clienteDB!=null && clienteDB.Email != null && clienteDB.Senha != null)
            {
                _loginCliente.Login(clienteDB);
                return new RedirectResult(Url.Action(nameof(Index)));
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

            return RedirectToAction(nameof(Index));
        }
        public IActionResult paginaAdm()
        {
            return View();
        }

        public IActionResult carrinhoDeCompra()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
