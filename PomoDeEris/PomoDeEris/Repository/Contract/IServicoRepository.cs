using PomoDeEris.Models;

namespace PomoDeEris.Repository.Contract
{
    public interface IServicoRepository
    {

        void Cadastrar(Servico servico);
        void Atualizar(Servico servico);

        void Excluir(int Id);

        Servico ObterServico(int Id);

        List<Servico> ObterTodosServicos();
    }
}
