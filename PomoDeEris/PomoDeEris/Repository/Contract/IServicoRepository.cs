using PomoDeEris.Models;

namespace PomoDeEris.Repository.Contract
{
    public interface IServicoRepository
    {

        void Cadastrar(Servico servico);
        void Atualizar(Servico servico);

        void Excluir(Servico servico);

        Servico ObterServico(int Id);

        List<Servico> ObterTodosServicos();
    }
}
