using PomoDeEris.Models;
using PomoDeEris.Areas.Colaborador.Models;
namespace PomoDeEris.Repository.Contract
{
    public interface IAtendimentoRepository
    {

        void Cadastrar(Atendimento atendimento, Pagamento pagamento);
        void Atualizar(Atendimento atendimento);

        void Excluir(Atendimento atendimento);

        Atendimento ObterAtendimento(int Id);

        List<Atendimento> ObterTodosAtendimentos();

        List<DateTime> ObterDiasBloqueados();

        List<DateTime> ObterHorariosBloqueados(DateTime data);

        List<Atendimento> ObterAtendimentosDia(DateTime data);

        List<int> ObterIdsConsultasDia(DateTime data);

        List<VendaPacote> ObterVendasPacotes();
        List<AtividadeRecente> ObterAtividadeRecente();

        int quantidadeConsultaHoje();

        double receitaMes();
        double receitaDia();

        int cadastrosHoje();


    }
}
