using MySql.Data.MySqlClient;
using PomoDeEris.Libraries.Carrinho;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Repository
{
    public class AgendaRepository : IAgendaRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _conf;
        private CarrinhoCookie _carrinhoCookie;

        public AgendaRepository(IConfiguration conf, CarrinhoCookie carrinhoCookie)
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _conf = conf;
        }

        public List<DateTime> ObterDiasBloqueados()
        {
            List<DateTime> dias = new List<DateTime>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(@"SELECT DiaHorario
                                                       FROM tbAgenda
                                                       WHERE tipo = 'dia'
                                                       AND DiaHorario >= CURDATE()", conexao);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        dias.Add(reader.GetDateTime("DiaHorario"));
                }
            }

            return dias;
        }

        public List<DateTime> ObterHorariosBloqueados(DateTime data)
        {
            List<DateTime> horarios = new List<DateTime>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(@"SELECT DiaHorario
                                                   FROM tbAgenda
                                                   WHERE tipo = 'horario'
                                                   AND DATE(DiaHorario) = @Data", conexao);

                cmd.Parameters.Add("@Data", MySqlDbType.Date).Value = data.Date;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        horarios.Add(reader.GetDateTime("DiaHorario"));
                }

            }

            return horarios;
        }
    }
}
