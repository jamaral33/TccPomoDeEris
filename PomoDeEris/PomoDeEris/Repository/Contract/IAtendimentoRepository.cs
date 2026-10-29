using PomoDeEris.Models;

namespace PomoDeEris.Repository.Contract
{
    public interface IAtendimentoRepository
    {

        void Cadastrar(Atendimento atendimento);
        void Atualizar(Atendimento atendimento);

        void Excluir(Atendimento atendimento);

        Atendimento ObterAtendimento(int Id);

        IEnumerable<Atendimento> ObterTodosAtendimentos();
    }
}
