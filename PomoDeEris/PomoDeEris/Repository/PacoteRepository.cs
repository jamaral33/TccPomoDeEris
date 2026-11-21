using MySql.Data.MySqlClient;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;
using System.Data;

namespace PomoDeEris.Repository
{
    public class PacoteRepository : IPacoteRepository
    {
        private readonly string _conexaoMySQL;


        public PacoteRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");

        }
        
        public void Cadastrar(Pacote pacote)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("CALL sp_cadastrar_pacote(@nome, @descricao, @valor, @Imagem)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = pacote.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = pacote.Descricao;
                cmd.Parameters.Add("@imagem", MySqlDbType.VarChar).Value = pacote.Imagem;
                cmd.Parameters.Add("@valor", MySqlDbType.Decimal).Value = pacote.Valor;
                cmd.ExecuteNonQuery();

                MySqlCommand cmdId = new MySqlCommand("SELECT LAST_INSERT_ID()", conexao);
                int idPacote = Convert.ToInt32(cmdId.ExecuteScalar());

                foreach (var servico in pacote.Servicos)
                {
                    MySqlCommand cmdServico = new MySqlCommand("CALL sp_cadastrar_servicosPacote(@idPacote, @idServico)", conexao);
                    cmdServico.Parameters.Add("@idPacote", MySqlDbType.Int32).Value = idPacote;
                    cmdServico.Parameters.Add("@idServico", MySqlDbType.Int32).Value = servico.IdServico;

                    cmdServico.ExecuteNonQuery();
                }
            }
        }

        public Pacote ObterPacote(int Id)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL)) 
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("CALL sp_selecionar_pacote(@idPacote)", conexao);
                cmd.Parameters.Add("@idPacote", MySqlDbType.Int32).Value = Id;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Pacote pacote = new Pacote();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    pacote.Nome = (String)(dr["Nome"]);
                    pacote.IdPacote = Id;
                    pacote.Descricao = (String)(dr["Descricao"]);
                    pacote.Imagem = (String)(dr["Imagem"]);
                    pacote.Valor = Convert.ToDouble(dr["Valor"]);


                }
                return pacote;
            }
        }

        public IEnumerable<Pacote> ObterTodosPacotes()
        {
            List<Pacote> Pacotelist = new List<Pacote>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("select * from tbPacote ", conexao);
                MySqlDataAdapter sd = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                sd.Fill(dt);
                conexao.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    Pacotelist.Add(
                         new Pacote
                         {
                             IdPacote = Convert.ToInt32(dr["idPacote"]),
                             Nome = (String)(dr["Nome"]),
                             Descricao = (String)(dr["Descricao"]),
                             Imagem = (String)(dr["Imagem"]),
                             Valor = Convert.ToDouble(dr["Valor"]),

                         });
                }
                return Pacotelist;
            }
        }
    }
    
}
