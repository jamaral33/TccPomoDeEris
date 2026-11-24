using PomoDeEris.Models;

namespace PomoDeEris.Repository.Contract
{
    public interface IPacoteRepository
    {
        Pacote ObterPacote(int Id);

        IEnumerable<Pacote> ObterTodosPacotes();

        void Cadastrar(Pacote pacote);

        void Atualizar(Pacote pacote);

        void Excluir(int Id);
    }
}
