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
        public Pacote ObterPacote(int Id)
        {
            throw new NotImplementedException();
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
