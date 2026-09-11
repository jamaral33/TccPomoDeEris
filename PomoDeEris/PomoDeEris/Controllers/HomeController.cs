using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Models;
using PomoDeEris.Repository;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IClienteRepository _clienteRepository;

        public HomeController(ILogger<HomeController> logger, IClienteRepository clienteRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult paginaDeLogin()
        {
            return View();
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
