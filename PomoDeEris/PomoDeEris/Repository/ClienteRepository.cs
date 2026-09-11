using MySql.Data.MySqlClient;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;
using System.Data;

namespace PomoDeEris.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly string _conexaoMySQL;

        IConfiguration _conf;

        public ClienteRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _conf = conf;

        }

        public void Atualizar(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(Cliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("insert into tbCliente (Nome, Nascimento, Sexo, CPF, Telefone, Email, Senha, Situacao)" +
                    "values (@Nome, @Nascimento, @Sexo, @CPF, @Telefone, @Email, @Senha, @Situacao)", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cliente.Nome;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = cliente.Senha;
 
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            throw new NotImplementedException();
        }

        public Cliente Login(string Email, string Senha)
        {
            throw new NotImplementedException();
        }

        public Cliente ObterCliente(int Id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Cliente> ObterTodosClientes()
        {
            throw new NotImplementedException();
        }
    }
}
