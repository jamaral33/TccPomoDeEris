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


        public ClienteController(ILogger<ClienteController> logger, IClienteRepository clienteRepository, LoginCliente loginCliente)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
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

        public IActionResult Historico()
        {
            return View();
        }

        public IActionResult infoCliente()
        {
            return View();
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
    }
}
