namespace PomoDeEris.Repository.Contract
{
    public interface IAgendaRepository
    {
        List<DateTime> ObterDiasBloqueados();

        List<DateTime> ObterHorariosBloqueados(DateTime data);
    }
}
