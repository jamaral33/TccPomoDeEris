using MySql.Data.MySqlClient;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

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
            throw new NotImplementedException();
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

        public void Excluir(Servico servico)
        {
            throw new NotImplementedException();
        }

        public Servico ObterServico(int Id)
        {
            throw new NotImplementedException();
        }

        public List<Servico> ObterTodosServicos()
        {
            throw new NotImplementedException();
        }
    }
}
