namespace PomoDeEris.Repository.Contract
{
    public interface IAgendaRepository
    {
        List<DateTime> ObterDiasBloqueados();

        List<DateTime> ObterHorariosBloqueados(DateTime data);

        bool AlternarDiaBloqueio(DateTime data);

        bool AlternarHorarioBloqueio(DateTime horario);

        bool VerificarHorarioDisponivel(DateTime horario, int idConsulta);
    }
}
