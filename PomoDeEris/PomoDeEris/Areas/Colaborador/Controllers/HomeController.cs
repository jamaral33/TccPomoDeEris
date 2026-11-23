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
        public IActionResult Index(DateTime? data)
        {
            if(data == null)
            {
                data = DateTime.Today;
            }

            ViewBag.DataSelecionada = data.Value.ToString("yyyy-MM-dd");

            DashBoardViewModel dashinfo = new DashBoardViewModel()
            {
                quantidadeConsultasHoje = _atendimentoRepository.QuantidadeConsultaHoje(),
                faturamentoDia = _atendimentoRepository.ReceitaDia(),
                faturamentoMes = _atendimentoRepository.ReceitaMes(),
                clientesCadastradosHoje = _atendimentoRepository.CadastrosHoje(),
                atividadesRecentes = _atendimentoRepository.ObterAtividadeRecente(),
                atendimentos = _atendimentoRepository.ObterAtendimentosDia(data.Value).OrderBy(a => a.DataHora).ToList(),
                vendasPacote = _atendimentoRepository.ObterVendasPacotes()


            };

            return View(dashinfo);
        }
    }
}
