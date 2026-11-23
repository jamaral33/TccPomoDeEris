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
        public IActionResult Index(DateTime? dia, string? procedimento, string? cliente, string? profissional, string? status)
        {
            var diaSelecionado = (dia ?? DateTime.Today).Date;

            ViewBag.DataSelecionada = diaSelecionado;
            ViewBag.DataSelecionadaISO = diaSelecionado.ToString("yyyy-MM-dd");

            List<Atendimento> atendimentos = _atendimentoRepository.ObterAtendimentosDia(diaSelecionado);

            if (!string.IsNullOrEmpty(procedimento))
                atendimentos = atendimentos.Where(a => a.PacotesAtendimento.Any(p => p.IdPacote.ToString() == procedimento)).ToList();

            if (!string.IsNullOrEmpty(cliente))
                atendimentos = atendimentos.Where(a => a.Cliente.CPF == cliente).ToList();

            if (!string.IsNullOrEmpty(profissional))
                atendimentos = atendimentos.Where(a => a.Funcionario.Matricula.ToString() == profissional).ToList();

            if (!string.IsNullOrEmpty(status))
                atendimentos = atendimentos.Where(a => a.Situacao.ToString() == status).ToList();

            return View(atendimentos.OrderBy(a => a.DataHora).ToList());
        }
    }
}
