using MySql.Data.MySqlClient;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;
using System.Data;
using PomoDeEris.Libraries.Criptografia;
using PomoDeEris.Models.Constants;

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
            var situacao = PessoaSituacaoConstant.Ativo;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                CriptografiaSenha criptografar = new CriptografiaSenha();
                string senha = criptografar.hasharSenha(cliente.Senha!);

                MySqlCommand cmd = new MySqlCommand("CALL sp_cadastrar_cliente(@CPF, @Nome, @Email, @Telefone, @Senha, @Situacao)", conexao); // ADD SITUACAO DPS

                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = cliente.CPF;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = cliente.Nome;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = cliente.Telefone;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = senha;
                cmd.Parameters.Add("@Situacao", MySqlDbType.VarChar).Value = situacao;

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
            CriptografiaSenha criptografar = new CriptografiaSenha();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("call sp_selecionar_clienteEmail(@Email)", conexao);

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Cliente cliente = new Cliente();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    cliente.Id = Convert.ToInt32(dr["idCliente"]);
                    cliente.Nome = Convert.ToString(dr["Nome"]);
                    cliente.CPF = Convert.ToString(dr["CPF"]);
                    cliente.Telefone = Convert.ToString(dr["Telefone"]);
                    cliente.Email = Convert.ToString(dr["Email"]);
                    cliente.Senha = Convert.ToString(dr["Senha"]);
                    cliente.Situacao = Convert.ToString(dr["Situacao"]);

                }
                if (cliente.CPF == null)
                {
                    return null;
                }

                byte[] salt = Convert.FromBase64String(cliente.Senha.Split('.', 2)[0]);

                if(cliente.Senha == criptografar.hasharSenhaSalt(Senha, salt))
                {
                    return cliente;

                }
                else
                {
                    return null;
                }
            }

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
