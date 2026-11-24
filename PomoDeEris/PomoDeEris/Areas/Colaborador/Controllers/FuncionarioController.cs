using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Areas.Colaborador.Models;
using PomoDeEris.Libraries.Filtro;
using PomoDeEris.Libraries.Login;
using PomoDeEris.Models;
using PomoDeEris.Models.Constants;
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

        [ColaboradorAutorizacao(FuncionarioCargoConstant.Administrador)]
        public IActionResult Index()
        {
            var funcionarios = _funcionarioRepository.ObterTodosFuncionarios();
            return View(funcionarios);
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

        [ColaboradorAutorizacao(FuncionarioCargoConstant.Administrador)]
        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [ColaboradorAutorizacao(FuncionarioCargoConstant.Administrador)]
        [HttpPost]
        public IActionResult Cadastrar([FromForm] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _funcionarioRepository.Cadastrar(funcionario);
                return RedirectToAction("Home", "Home", new { area = "Colaborador" });
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            var funcionario = _funcionarioRepository.ObterFuncionario(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult Atualizar([FromForm] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _funcionarioRepository.Atualizar(funcionario);
                return RedirectToAction("Index", "Funcionario", new { area = "Colaborador" });
            }
            else
            {
                return View(funcionario);
            }
        }

    }
}
