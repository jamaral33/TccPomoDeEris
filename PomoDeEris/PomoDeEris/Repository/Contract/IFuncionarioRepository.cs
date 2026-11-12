using PomoDeEris.Models;

namespace PomoDeEris.Repository.Contract
{
    public interface IFuncionarioRepository
    {
        Funcionario Login(string Email, string Senha);

    }
}
