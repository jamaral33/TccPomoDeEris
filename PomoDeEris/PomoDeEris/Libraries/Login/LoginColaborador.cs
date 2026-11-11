using Newtonsoft.Json;
using PomoDeEris.Models;

namespace PomoDeEris.Libraries.Login
{
    public class LoginFuncionario
    {
        private string Key = "Login.Funcionario";
        private Sessao.Sessao _sessao;
        public LoginFuncionario(Sessao.Sessao sessao)
        {
            _sessao = sessao;
        }
        public void Login(Funcionario Funcionario)
        {
            string FuncionarioJSONString = JsonConvert.SerializeObject(Funcionario);

            _sessao.Cadastrar(Key, FuncionarioJSONString);
        }

        public Funcionario GetFuncionario()
        {
            // Deserializar
            if (_sessao.Existe(Key))
            {
                string FuncionarioJSONString = _sessao.Consultar(Key);
                return JsonConvert.DeserializeObject<Funcionario>(FuncionarioJSONString);
            }
            else
            {
                return null;
            }
        }
        //Remove a sessão e desloga o Funcionario
        public void Logout()
        {
            _sessao.RemoverTodos();
        }


    }
}
