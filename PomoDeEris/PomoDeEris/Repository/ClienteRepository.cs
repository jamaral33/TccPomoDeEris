using MySql.Data.MySqlClient;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;
using System.Data;
using PomoDeEris.Libraries.Criptografia;

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
                CriptografiaSenha criptografar = new CriptografiaSenha();
                string senha = criptografar.hasharSenha(cliente.Senha!);

                MySqlCommand cmd = new MySqlCommand("CALL sp_cadastrar_cliente(@CPF, @Nome, @Email, @Telefone, @Senha)", conexao);

                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = cliente.CPF;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cliente.Nome;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = senha;
 
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
