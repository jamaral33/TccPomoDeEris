using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Carrinho;
using PomoDeEris.Libraries.Login;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Controllers
{
    public class AtendimentoController : Controller
    {

        private IAtendimentoRepository _atendimentoRepository;
        private CarrinhoCookie _carrinhoCookie;
        private LoginCliente _loginCliente;

        public AtendimentoController(IAtendimentoRepository atendimentoRepository, CarrinhoCookie carrinhoCookie, LoginCliente loginCliente)
        {
            _atendimentoRepository = atendimentoRepository;
            _carrinhoCookie = carrinhoCookie;
            _loginCliente = loginCliente;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmarHorario()
        {
            if(_loginCliente.GetCliente() == null)
            {
                return RedirectToAction("Cadastro", "Cliente");
            }
            if(_carrinhoCookie.ContarItens() == 0)
            {
                return RedirectToAction("Index", "Carrinho");
            }
            //VERIFICAR SE O CARRINHO N ESTÁ VAZIO E O USUARIO LOGADO !!!!

            return View();
        }
        [HttpPost]
        public IActionResult Agendar(DateTime horario)
        {
            Atendimento atendimento = new Atendimento()
            {
                dataHora = horario,
                cliente = _loginCliente.GetCliente()
            };

            _atendimentoRepository.Cadastrar(atendimento);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Detalhes(int id)
        {
            return View();
        }
    }
}
