using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Filtro;
using PomoDeEris.Models;
using PomoDeEris.Repository;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    [ColaboradorAutorizacaoAttribute]
    public class AtendimentoController : Controller
    {
        IAtendimentoRepository _atendimentoRepository;
        IAgendaRepository _agendaRepository;
        public AtendimentoController(IAtendimentoRepository atendimentoRepository, IAgendaRepository agendaRepository)
        {
            _atendimentoRepository = atendimentoRepository;
            _agendaRepository = agendaRepository;
        }

        [HttpGet]
        public IActionResult Index(DateTime? dia, string? procedimento, string? cliente, string? profissional, string? status)
        {
            var diaSelecionado = (dia ?? DateTime.Today).Date;

            ViewBag.DataSelecionada = diaSelecionado;
            ViewBag.DataSelecionadaISO = diaSelecionado.ToString("yyyy-MM-dd");

            List<Atendimento> atendimentos = _atendimentoRepository.ObterAtendimentosDia(diaSelecionado);

            if (!string.IsNullOrEmpty(procedimento)) //Verifica se o filtro está nulo
                atendimentos = atendimentos.Where(a => a.PacotesAtendimento.Any(p => p.IdPacote.ToString() == procedimento)).ToList(); //filtra

            if (!string.IsNullOrEmpty(cliente))
                atendimentos = atendimentos.Where(a => a.Cliente.CPF == cliente).ToList();

            if (!string.IsNullOrEmpty(profissional))
                atendimentos = atendimentos.Where(a => a.Funcionario.Matricula.ToString() == profissional).ToList();

            if (!string.IsNullOrEmpty(status))
                atendimentos = atendimentos.Where(a => a.Situacao.ToString() == status).ToList();

            return View(atendimentos.OrderBy(a => a.DataHora).ToList());
        }

        [HttpGet]
        public IActionResult Atualizar(int id)
        {
            Atendimento atendimento = _atendimentoRepository.ObterAtendimento(id);
            return View(atendimento);
        }
        [HttpPost]
        public IActionResult Atualizar(Atendimento atendimento)
        {
            if (atendimento.Situacao != "Cancelado" && !_agendaRepository.VerificarHorarioDisponivel(atendimento.DataHora, atendimento.IdConsulta))
            {
                TempData["Erro"] = "Esse horário não está disponível.";
                return RedirectToAction("Atualizar", new { id = atendimento.IdConsulta });
            }

            _atendimentoRepository.Atualizar(atendimento);
                return RedirectToAction("Index");
        }
    }
}
