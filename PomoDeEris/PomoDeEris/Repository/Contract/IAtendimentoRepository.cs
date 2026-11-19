using PomoDeEris.Models;

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

        int quantidadeConsultaHoje();

        double receitaMes();
        double receitaDia();

        int cadastrosHoje();


    }
}
