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
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("CALL sp_cadastrar_pacote(@nome, @descricao, @valor, @desconto, @Imagem)", conexao);
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = pacote.Nome;
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = pacote.Descricao;
                cmd.Parameters.Add("@imagem", MySqlDbType.VarChar).Value = pacote.Imagem;
                cmd.Parameters.Add("@valor", MySqlDbType.Decimal).Value = pacote.Valor;
                cmd.Parameters.Add("@desconto", MySqlDbType.Decimal).Value = pacote.Desconto;
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
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("CALL sp_selecionar_pacote(@idPacote)", conexao);
                cmd.Parameters.Add("@idPacote", MySqlDbType.Int32).Value = Id;


                Pacote pacote = new Pacote();
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        pacote.Nome = dr["Nome"].ToString();
                        pacote.IdPacote = Convert.ToInt32(dr["idPacote"]);
                        pacote.Descricao = dr["Descricao"].ToString();
                        pacote.Imagem = dr["Imagem"].ToString();
                        pacote.Valor = Convert.ToDouble(dr["Valor"]);
                        pacote.Desconto = Convert.ToInt32(dr["Desconto"]);
                    }
                }


                MySqlCommand cmdServicos = new MySqlCommand("CALL sp_selecionar_servicos_pacote(@idPacote)", conexao);
                cmdServicos.Parameters.Add("@idPacote", MySqlDbType.Int32).Value = Id;

                MySqlDataAdapter da = new MySqlDataAdapter(cmdServicos);

                DataTable dt = new DataTable();
                da.Fill(dt);
                conexao.Close();

                List<Servico> servicos = new List<Servico>();

                foreach (DataRow dataRow in dt.Rows)
                {
                    servicos.Add(
                         new Servico
                         {
                             IdServico = Convert.ToInt32(dataRow["idServico"]),
                             NomeServico = (String)(dataRow["Nome"]),
                             DescricaoServico = (String)(dataRow["Descricao"])
                         });
                }

                pacote.Servicos = servicos;
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
                foreach (DataRow dr in dt.Rows)
                {
                    int idPacote = Convert.ToInt32(dr["idPacote"]);
                    Pacotelist.Add(
                         new Pacote
                         {
                             IdPacote = idPacote,
                             Nome = (String)(dr["Nome"]),
                             Descricao = (String)(dr["Descricao"]),
                             Imagem = (String)(dr["Imagem"]),
                             Valor = Convert.ToDouble(dr["Valor"]),
                             Desconto = Convert.ToInt32(dr["Desconto"]),
                             Servicos = ObterServicosDoPacote(conexao, idPacote)
                         });
                }
                conexao.Close();
                return Pacotelist;
            }
        }
        public void Atualizar(Pacote pacote)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "CALL sp_atualizar_pacote(@IdPacote, @Nome, @Descricao, @Valor, @Desconto, @Imagem)",
                    conexao
                );

                cmd.Parameters.Add("@IdPacote", MySqlDbType.Int32).Value = pacote.IdPacote;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = pacote.Nome;
                cmd.Parameters.Add("@Descricao", MySqlDbType.VarChar).Value = pacote.Descricao;
                cmd.Parameters.Add("@Valor", MySqlDbType.Decimal).Value = pacote.Valor;
                cmd.Parameters.Add("@Desconto", MySqlDbType.Int32).Value = pacote.Desconto;
                cmd.Parameters.Add("@Imagem", MySqlDbType.VarChar).Value = pacote.Imagem ?? "";

                cmd.ExecuteNonQuery();

                //apaga os antigos e cadastra os selecionados
                MySqlCommand cmdRemover = new MySqlCommand(
                    "CALL sp_remover_servicos_pacote(@IdPacote)", conexao
                );
                cmdRemover.Parameters.Add("@IdPacote", MySqlDbType.Int32).Value = pacote.IdPacote;
                cmdRemover.ExecuteNonQuery();

                foreach (var servico in pacote.Servicos)
                {
                    MySqlCommand cmdServico = new MySqlCommand(
                        "CALL sp_cadastrar_servicosPacote(@IdPacote, @IdServico)", conexao
                    );
                    cmdServico.Parameters.Add("@IdPacote", MySqlDbType.Int32).Value = pacote.IdPacote;
                    cmdServico.Parameters.Add("@IdServico", MySqlDbType.Int32).Value = servico.IdServico;
                    cmdServico.ExecuteNonQuery();
                }

                conexao.Close();
            }
        }

        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {

                conexao.Open();
                MySqlCommand cmdServico = new MySqlCommand("DELETE FROM tbServicosPacote WHERE idPacote = @Id", conexao);
                cmdServico.Parameters.Add("@Id", MySqlDbType.Int32).Value = Id;
                cmdServico.ExecuteNonQuery();

                MySqlCommand cmd = new MySqlCommand("delete from tbPacote WHERE IdPacote = @Id", conexao);
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public List<Pacote> PesquisarPacotes(string pesquisa)
        {
            List<Pacote> pacotes = new List<Pacote>();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("call sp_pesquisar_pacotes(@pesquisa)", conexao);
                cmd.Parameters.Add("@pesquisa", MySqlDbType.VarChar).Value = pesquisa;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow dataRow in dt.Rows)
                {
                    int idPacote = Convert.ToInt32(dataRow["idPacote"]);
                    pacotes.Add(
                        new Pacote
                        {
                            IdPacote = idPacote,
                            Nome = (string)(dataRow["Nome"]),
                            Descricao = (string)(dataRow["Descricao"]),
                            Valor = Convert.ToDouble(dataRow["Valor"]),
                            Desconto = Convert.ToInt32(dataRow["Desconto"]),
                            Imagem = (string)(dataRow["Imagem"]),
                            Servicos = ObterServicosDoPacote(conexao, idPacote)
                        });

                }

                return pacotes;
            }
        }

        // Busca os serviços de um pacote usando a conexão que já está aberta
        private List<Servico> ObterServicosDoPacote(MySqlConnection conexao, int idPacote)
        {
            MySqlCommand cmd = new MySqlCommand("CALL sp_selecionar_servicos_pacote(@idPacote)", conexao);
            cmd.Parameters.Add("@idPacote", MySqlDbType.Int32).Value = idPacote;

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            List<Servico> servicos = new List<Servico>();
            foreach (DataRow dataRow in dt.Rows)
            {
                servicos.Add(
                    new Servico
                    {
                        IdServico = Convert.ToInt32(dataRow["idServico"]),
                        NomeServico = (string)(dataRow["Nome"]),
                        DescricaoServico = (string)(dataRow["Descricao"])
                    });
            }
            return servicos;
        }
    }
}