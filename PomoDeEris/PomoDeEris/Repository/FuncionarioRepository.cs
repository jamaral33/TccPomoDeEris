using MySql.Data.MySqlClient;
using PomoDeEris.Libraries.Criptografia;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;
using System.Data;

namespace PomoDeEris.Repository
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private string _conexaoMySQL;
        private IConfiguration _conf;


        public FuncionarioRepository(IConfiguration conf)
        {
            _conexaoMySQL =  conf.GetConnectionString("ConexaoMySQL");
            _conf = conf;
        }

        public Funcionario Login(string Email, string Senha)
        {
            CriptografiaSenha criptografar = new CriptografiaSenha();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("call sp_selecionar_funcionarioEmail(@Email)", conexao);

                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = Email;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Funcionario funcionario = new Funcionario();

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    funcionario.Nome = Convert.ToString(dr["Nome"]);
                    funcionario.CPF = Convert.ToString(dr["CPF"]);
                    funcionario.Telefone = Convert.ToString(dr["Telefone"]);
                    funcionario.Email = Convert.ToString(dr["Email"]);
                    funcionario.Senha = Convert.ToString(dr["Senha"]);
                    funcionario.Situacao = Convert.ToString(dr["Situacao"]);
                    funcionario.Cargo = Convert.ToString(dr["Funcao"]);
                    funcionario.Matricula = Convert.ToInt32(dr["Matricula"]);


                }
                if (funcionario.CPF == null)
                {
                    return null;
                }

                byte[] salt = Convert.FromBase64String(funcionario.Senha.Split('.', 2)[0]);

                if (funcionario.Senha == criptografar.hasharSenhaSalt(Senha, salt))
                {
                    return funcionario;

                }
                else
                {
                    return null;
                }
            }

        }
    }
}
