using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using PomoDeEris.Libraries.Carrinho;
using PomoDeEris.Libraries.Criptografia;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Repository
{
    public class AtendimentoRepository : IAtendimentoRepository
    {
        private readonly string _conexaoMySQL;
        IConfiguration _conf;
        private CarrinhoCookie _carrinhoCookie;

        public AtendimentoRepository(IConfiguration conf, CarrinhoCookie carrinhoCookie )
        {
            _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");
            _conf = conf;
            _carrinhoCookie = carrinhoCookie;
        }

        public void Atualizar(Atendimento atendimento)
        {
            throw new NotImplementedException();
        }

        public void Cadastrar(Atendimento atendimento, Pagamento pagamento)
        {
            List<Pacote> pacotesConsulta = _carrinhoCookie.Consultar();
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Insere a consulta
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("CALL sp_cadastrar_consulta(@IdCliente, @Horario)", conexao); 

                cmd.Parameters.Add("@IdCliente", MySqlDbType.Int32).Value = atendimento.cliente.Id;
                cmd.Parameters.Add("@Horario", MySqlDbType.DateTime).Value = atendimento.dataHora;
                cmd.ExecuteNonQuery();

                //pega o id da consulta que acabou de ser inserida
                MySqlCommand cmdId = new MySqlCommand("SELECT LAST_INSERT_ID()", conexao);

                int idConsulta = Convert.ToInt32(cmdId.ExecuteScalar());

                foreach (var pacote in pacotesConsulta)
                {
                    MySqlCommand cmd2 = new MySqlCommand("CALL sp_cadastrar_pacote_consulta (@IdConsulta, @IdPacote, @Quantidade)", conexao);
                    cmd2.Parameters.Add("@IdConsulta", MySqlDbType.Int32).Value = idConsulta;
                    cmd2.Parameters.Add("@IdPacote", MySqlDbType.Int32).Value = pacote.IdPacote;
                    cmd2.Parameters.Add("@Quantidade", MySqlDbType.Int16).Value = pacote.Quantidade;

                    cmd2.ExecuteNonQuery();
                }

                MySqlCommand cmdPagamento = new MySqlCommand("CALL sp_cadastrar_pagamento(@idConsulta, @Valor, @Metodo, @Situacao)", conexao);
                cmdPagamento.Parameters.Add("@idConsulta", MySqlDbType.Int32).Value = idConsulta;
                cmdPagamento.Parameters.Add("@Valor", MySqlDbType.Decimal).Value = pagamento.Valor;
                cmdPagamento.Parameters.Add("@Metodo", MySqlDbType.VarChar).Value = pagamento.Metodo;
                cmdPagamento.Parameters.Add("@Situacao", MySqlDbType.VarChar).Value = pagamento.Status;

                cmdPagamento.ExecuteNonQuery();

                _carrinhoCookie.RemoverTodos();

                conexao.Close();
            }
        }


        public void Excluir(Atendimento atendimento)
        {
            throw new NotImplementedException();
        }

        public Atendimento ObterAtendimento(int Id)
        {
            throw new NotImplementedException();
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

        public IEnumerable<Atendimento> ObterTodosAtendimentos()
        {
            throw new NotImplementedException();
        }

        public int quantidadeConsultaHoje()
        {
            var quantidade = 0;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "CALL sp_selecionar_quantidade_consultas_hoje()", conexao);

                var resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    quantidade = Convert.ToInt32(resultado);
                }

            }

            return quantidade;
        }

        public double receitaDia()
        {
            double receita = 0;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "CALL sp_selecionar_receita_dia()", conexao);

                var resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    receita = Convert.ToDouble(resultado);
                }

            }

            return receita;
        }

        public double receitaMes()
        {
            double receita = 0;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "CALL sp_selecionar_receita_mes()", conexao);

                var resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    receita = Convert.ToDouble(resultado);
                }

            }

            return receita;
        }

        public int cadastrosHoje()
        {
            int cadastros = 0;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand(
                    "CALL sp_selecionar_quantidade_clientes_hoje()", conexao);

                var resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    cadastros = Convert.ToInt32(resultado);
                }

            }

            return cadastros;
        }
    }
}
