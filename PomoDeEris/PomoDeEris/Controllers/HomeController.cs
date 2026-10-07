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
        private IPacoteRepository _pacoteRepository;



        public HomeController(ILogger<HomeController> logger, IClienteRepository clienteRepository, LoginCliente loginCliente, IPacoteRepository pacoteRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
            _loginCliente = loginCliente;
            _pacoteRepository = pacoteRepository;
        }

        public IActionResult Index()
        {
            return View(_pacoteRepository.ObterTodosPacotes());
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
