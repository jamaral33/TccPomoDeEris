using PomoDeEris.Models;

namespace PomoDeEris.Repository.Contract
{
    public interface IPacoteRepository
    {
        Pacote ObterPacote(int Id);

        IEnumerable<Pacote> ObterTodosPacotes();
    }
}
