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

                MySqlCommand cmd = new MySqlCommand(@"
                        SELECT a.DiaHorario
                        FROM tbAgenda a
                        LEFT JOIN tbConsulta c ON c.idConsulta = a.idConsulta
                        WHERE a.tipo = 'horario'
                          AND DATE(a.DiaHorario) = @Data
                          AND (a.idConsulta IS NULL OR c.Situacao != 'Cancelado')", conexao);

                cmd.Parameters.Add("@Data", MySqlDbType.Date).Value = data.Date;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        horarios.Add(reader.GetDateTime("DiaHorario"));
                }

            }

            return horarios;
        }

        // false = tem consulta ativa no dia (não mexe), true = bloqueou ou liberou.
        public bool AlternarDiaBloqueio(DateTime data)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                //não deixa bloquear/liberar se tem consulta ativa em qualquer horário do dia
                var cmdConsulta = new MySqlCommand(@"
                    SELECT COUNT(*)
                    FROM tbAgenda a
                    JOIN tbConsulta c ON c.idConsulta = a.idConsulta
                    WHERE c.Situacao != 'Cancelado'
                      AND DATE(a.DiaHorario) = @Data", conexao);

                cmdConsulta.Parameters.Add("@Data", MySqlDbType.Date).Value = data.Date;

                if (Convert.ToInt32(cmdConsulta.ExecuteScalar()) > 0)
                    return false;

                //tenta liberar (deletar o registro), se não apagou nada, bloqueia (inseri o registro)
                var cmdDelete = new MySqlCommand(@" DELETE FROM tbAgenda
                                                    WHERE tipo = 'dia' AND idConsulta IS NULL AND DATE(DiaHorario) = @Data", conexao);

                cmdDelete.Parameters.Add("@Data", MySqlDbType.Date).Value = data.Date;

                if (cmdDelete.ExecuteNonQuery() == 0)
                {
                    var cmdInsert = new MySqlCommand(@"
                                                    INSERT INTO tbAgenda (DiaHorario, tipo)
                                                    VALUES (@Data, 'dia')", conexao);
                    cmdInsert.Parameters.Add("@Data", MySqlDbType.DateTime).Value = data.Date;
                    cmdInsert.ExecuteNonQuery();
                }

                return true;
            }
        }

        // false = tem consulta ativa no dia (não mexe), true = bloqueou ou liberou.
        public bool AlternarHorarioBloqueio(DateTime horario)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // 1. não deixa bloquear/liberar se tem consulta ativa nesse horário
                var cmdConsulta = new MySqlCommand(@"
            SELECT COUNT(*)
            FROM tbAgenda a
            JOIN tbConsulta c ON c.idConsulta = a.idConsulta
            WHERE c.Situacao != 'Cancelado'
              AND a.DiaHorario = @Horario", conexao);
                cmdConsulta.Parameters.Add("@Horario", MySqlDbType.DateTime).Value = horario;

                if (Convert.ToInt32(cmdConsulta.ExecuteScalar()) > 0)
                    return false;

                // 2. tenta liberar (DELETE); se não apagou nada, bloqueia (INSERT)
                var cmdDelete = new MySqlCommand(@"
            DELETE FROM tbAgenda
            WHERE tipo = 'horario' AND idConsulta IS NULL AND DiaHorario = @Horario", conexao);
                cmdDelete.Parameters.Add("@Horario", MySqlDbType.DateTime).Value = horario;

                if (cmdDelete.ExecuteNonQuery() == 0)
                {
                    var cmdInsert = new MySqlCommand(@"
                INSERT INTO tbAgenda (DiaHorario, tipo)
                VALUES (@Horario, 'horario')", conexao);
                    cmdInsert.Parameters.Add("@Horario", MySqlDbType.DateTime).Value = horario;
                    cmdInsert.ExecuteNonQuery();
                }

                return true;
            }
        }
    }
}
