using MySql.Data.MySqlClient;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;
using System.Data;

namespace PomoDeEris.Repository
{

    public class ServicoRepository : IServicoRepository
    {
        private IConfiguration _conf;
        private string _conexaoMySQL;

        public ServicoRepository(IConfiguration conf)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _conf = conf;
        }
        public void Atualizar(Servico servico)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE tbServico SET Nome=@Nome, Descricao=@Descricao WHERE idServico=@IdServico",
                    conexao
                );

                cmd.Parameters.Add("@IdServico", MySqlDbType.Int32).Value = servico.IdServico;
                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = servico.NomeServico;
                cmd.Parameters.Add("@Descricao", MySqlDbType.VarChar).Value = servico.DescricaoServico;

                cmd.ExecuteNonQuery();
                conexao.Close();
            }
        }

        public void Cadastrar(Servico servico)
        {
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("CALL sp_cadastrar_servico(@Nome, @Descricao)", conexao);

                cmd.Parameters.Add("@Nome", MySqlDbType.VarChar).Value = servico.NomeServico;
                cmd.Parameters.Add("@Descricao", MySqlDbType.VarChar).Value = servico.DescricaoServico;

                cmd.ExecuteNonQuery();
                conexao.Close();

            }
        }

        public void Excluir(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("delete from tbservico WHERE IdServico = @Id", conexao);
                cmd.Parameters.AddWithValue("@Id", Id);

                cmd.ExecuteNonQuery();

                conexao.Close();
            }
        }

        public Servico ObterServico(int Id)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("select * from tbservico where idservico = @Id", conexao);
                cmd.Parameters.Add("@Id", MySqlDbType.Int32).Value = Id;

                MySqlDataReader dr;

                Servico servico = new Servico();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    servico.NomeServico = (String)(dr["Nome"]);
                    servico.IdServico = Id;
                    servico.DescricaoServico = (String)(dr["Descricao"]);


                }
                return servico;
            }
        }

        public List<Servico> ObterTodosServicos()
        {
            List<Servico> servicos = new List<Servico>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tbservico", conexao);


                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        servicos.Add(new Servico
                        {
                            IdServico = Convert.ToInt32(reader["IdServico"]),
                            NomeServico = (string)reader["Nome"],
                            DescricaoServico = (string)reader["Descricao"]
                        });
                    }
                    }
                conexao.Close();

            }
            return servicos;
        }
    }
}
