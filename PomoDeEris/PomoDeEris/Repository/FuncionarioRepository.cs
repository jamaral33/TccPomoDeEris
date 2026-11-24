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

        public void Atualizar(Funcionario funcionario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("CALL sp_atualizar_funcionario(@Matricula, @CPF, @Nome, @Email, @Situacao, @Telefone, @Senha, @Funcao)", conexao);

                cmd.Parameters.Add("@Matricula", MySqlDbType.Int32).Value = funcionario.Matricula;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = funcionario.CPF;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = funcionario.Telefone;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = funcionario.Email;
                cmd.Parameters.Add("@Situacao", MySqlDbType.VarChar).Value = funcionario.Situacao;
                cmd.Parameters.Add("@Funcao", MySqlDbType.VarChar).Value = funcionario.Cargo;

                if (!string.IsNullOrWhiteSpace(funcionario.Senha))
                {
                    CriptografiaSenha criptografar = new CriptografiaSenha();
                    string senhaCriptografada = criptografar.hasharSenha(funcionario.Senha);

                    cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = senhaCriptografada;
                }
                else
                {
                    cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = DBNull.Value; // No banco ele mantem a senha se for nulo
                }

                cmd.ExecuteNonQuery();
            }
        }

        public void Cadastrar(Funcionario funcionario)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call sp_cadastrar_funcionario(@CPF, @Nome, @Email, @Situacao, @Telefone, @Senha, @Funcao)", conexao);

                CriptografiaSenha criptografar = new CriptografiaSenha();
                string senhaCriptografada = criptografar.hasharSenha(funcionario.Senha);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = funcionario.Nome;
                cmd.Parameters.Add("@CPF", MySqlDbType.VarChar).Value = funcionario.CPF;
                cmd.Parameters.Add("@Telefone", MySqlDbType.VarChar).Value = funcionario.Telefone;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar).Value = funcionario.Email;
                cmd.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = senhaCriptografada;
                cmd.Parameters.Add("@Situacao", MySqlDbType.VarChar).Value = funcionario.Situacao;
                cmd.Parameters.Add("@Funcao", MySqlDbType.VarChar).Value = funcionario.Cargo;

                cmd.ExecuteNonQuery();
            }
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

        public Funcionario ObterFuncionario(int id)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call sp_selecionar_funcionario(@Matricula)", conexao);

                cmd.Parameters.Add("@Matricula", MySqlDbType.Int32).Value = id;
                MySqlDataReader dr;

                Funcionario funcionario = new Funcionario();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    funcionario.Matricula = Convert.ToInt32(dr["Matricula"]);
                    funcionario.Nome = Convert.ToString(dr["Nome"]);
                    funcionario.CPF = Convert.ToString(dr["CPF"]);
                    funcionario.Telefone = Convert.ToString(dr["Telefone"]);
                    funcionario.Email = Convert.ToString(dr["Email"]);
                    funcionario.Situacao = Convert.ToString(dr["Situacao"]);
                    funcionario.Cargo = Convert.ToString(dr["Funcao"]);
                }
                return funcionario;
            }
        }

        public List<Funcionario> ObterTodosFuncionarios()
        {
            List<Funcionario> funcionarios = new List<Funcionario>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("call sp_selecionar_todos_funcionarios()", conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                DataTable dt = new DataTable();

                da.Fill(dt);
                conexao.Close();
                foreach (DataRow dateRow in dt.Rows)
                {
                    funcionarios.Add(
                         new Funcionario
                         {
                             Matricula = Convert.ToInt32(dateRow["Matricula"]),
                             Nome = (String)(dateRow["Nome"]),
                             Email = (String)(dateRow["Email"]),
                             Cargo = (String)(dateRow["Funcao"]),
                             Telefone = (String)(dateRow["Telefone"]),
                             CPF = (String)(dateRow["CPF"]),
                             Situacao = (String)(dateRow["Situacao"]),

                         });
                }
            }
            return funcionarios;
        }
    }
}
