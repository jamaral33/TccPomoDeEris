using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Filtro;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacaoAttribute]
    public class AtendimentoController : Controller
    {
        IAtendimentoRepository _atendimentoRepository;
        public AtendimentoController(IAtendimentoRepository atendimentoRepository)
        {
            _atendimentoRepository = atendimentoRepository;
        }

        [HttpGet]
        public IActionResult Index(DateTime? dia, string? profissional, string? status)
        {

            DateTime dataFiltro;

            if(dia == null)
            {
                dataFiltro = DateTime.Today;
            }
            else
            {
                dataFiltro = dia.Value;
            }

            List<Atendimento> atendimentos = _atendimentoRepository.ObterAtendimentosDia(dataFiltro);

            if(profissional != null)
            {
                atendimentos = atendimentos.Where(a => a.Funcionario.Nome  == profissional).ToList();
            }
            if(status != null)
            {
                atendimentos = atendimentos.Where(a => a.Situacao == status).ToList();
            }

            ViewBag.dataSelecionada = dataFiltro.ToString("yyyy-MM-dd");
            return View(atendimentos);
        }
    }
}
