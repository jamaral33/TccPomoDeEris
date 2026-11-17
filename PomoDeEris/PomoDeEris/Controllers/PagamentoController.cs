using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Carrinho;
using PomoDeEris.Libraries.Login;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Controllers
{
    public class PagamentoController : Controller
    {

        private LoginCliente _loginCliente;

        public PagamentoController(LoginCliente loginCliente)
        {
            _loginCliente = loginCliente;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Pagar(DateTime horario)
        {
            var cliente = _loginCliente.GetCliente();
            return View(horario);
        }



    }
}
