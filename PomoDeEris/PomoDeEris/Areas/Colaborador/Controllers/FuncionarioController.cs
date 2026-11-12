using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Areas.Colaborador.Models;
using PomoDeEris.Libraries.Login;
using PomoDeEris.Models;
using PomoDeEris.Repository;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]

    public class FuncionarioController : Controller
    {
        private IFuncionarioRepository _funcionarioRepository;
        private LoginFuncionario _loginFuncionario;
        public FuncionarioController(IFuncionarioRepository funcionarioRepository, LoginFuncionario loginFuncionario)
        {
            _funcionarioRepository = funcionarioRepository;
            _loginFuncionario = loginFuncionario;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromForm] LoginFuncionarioViewModel funcionario)
        {
            Funcionario funcionarioDB = _funcionarioRepository.Login(funcionario.Email, funcionario.Senha);
            if (funcionarioDB != null && funcionarioDB.Email != null && funcionarioDB.Senha != null)
            {
                _loginFuncionario.Login(funcionarioDB);
                return RedirectToAction("Index", "Home", new {area = "Colaborador"});
            }
            else
            {
                //ERRO NA SESSAO
                ViewData["MSG_E"] = "Usuário não localizado, por favor verifique o email e senha digitado";
                return View();
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
    }
}
