using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Areas.Colaborador.Models;
using PomoDeEris.Libraries.Carrinho;
using PomoDeEris.Libraries.Filtro;
using PomoDeEris.Libraries.Login;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class HomeController : Controller
    {
        private IAtendimentoRepository _atendimentoRepository;

        public HomeController(IAtendimentoRepository atendimentoRepository)
        {
            _atendimentoRepository = atendimentoRepository;
        }

        [ColaboradorAutorizacaoAttribute]
        public IActionResult Index()
        {

            DashBoardViewModel dashinfo = new DashBoardViewModel()
            {
                quantidadeConsultasHoje = _atendimentoRepository.quantidadeConsultaHoje(),
                faturamentoDia = _atendimentoRepository.receitaDia(),
                faturamentoMes = _atendimentoRepository.receitaMes(),
                clientesCadastradosHoje = _atendimentoRepository.cadastrosHoje(),
                
                atendimentos = _atendimentoRepository.ObterAtendimentosDia(DateTime.Now),
                vendasPacote = _atendimentoRepository.ObterVendasPacotes()


            };

            return View(dashinfo);
        }
    }
}
