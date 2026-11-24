using PomoDeEris.Models;

namespace PomoDeEris.Repository.Contract
{
    public interface IFuncionarioRepository
    {
        Funcionario Login(string Email, string Senha);

        Funcionario ObterFuncionario(int id);

        List<Funcionario> ObterTodosFuncionarios();

        void Cadastrar(Funcionario funcionario);

        void Atualizar(Funcionario funcionario);

    }
}
