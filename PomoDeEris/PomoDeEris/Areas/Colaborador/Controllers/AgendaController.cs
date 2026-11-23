using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Filtro;
using PomoDeEris.Repository.Contract;

[Area("Colaborador")]
[ColaboradorAutorizacaoAttribute]
public class AgendaController : Controller
{
    private IAgendaRepository _agendaRepository;

    public AgendaController(IAgendaRepository agendaRepository)
    {
        _agendaRepository = agendaRepository;
    }

    public IActionResult Index(DateTime? data)
    {
        // guarda a data selecionada para o calendário continuar nela depois do POST
        ViewBag.Data = data?.ToString("yyyy-MM-dd");

        var dias = _agendaRepository.ObterDiasBloqueados();
        return View(dias);
    }

    [HttpGet]
    public IActionResult ObterHorarios(DateTime data)
    {
        var horarios = _agendaRepository.ObterHorariosBloqueados(data);
        return Json(horarios.Select(h => h.ToString("HH:mm")));
    }

    [HttpPost]
    public IActionResult BloquearDia(DateTime data)
    {
        if (!_agendaRepository.AlternarDiaBloqueio(data))
            TempData["Erro"] = "Existe consulta marcada neste dia. Cancele antes de bloquear.";

        return RedirectToAction(nameof(Index), new { data = data.ToString("yyyy-MM-dd") });
    }

    [HttpPost]
    public IActionResult BloquearHorario(DateTime horario)
    {
        if (!_agendaRepository.AlternarHorarioBloqueio(horario))
            TempData["Erro"] = "Existe consulta marcada neste horário. Cancele antes de bloquear.";

        return RedirectToAction(nameof(Index), new { data = horario.ToString("yyyy-MM-dd") });
    }


}