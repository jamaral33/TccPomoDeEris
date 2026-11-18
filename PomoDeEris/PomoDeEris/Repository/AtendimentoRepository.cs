using System.Data;
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
            Atendimento atendimento = new Atendimento();
            List<Pacote> pacotesAtendimento = new List<Pacote>();
            atendimento.cliente = new Cliente();
            atendimento.funcionario = new Funcionario();
            atendimento.pagamento = new Pagamento();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(@"CALL sp_selecionar_informacoes_consulta(@idConsulta)", conexao);
                cmd.Parameters.Add("@idConsulta", MySqlDbType.Int32).Value = Id;

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                MySqlDataReader dr;

                Pacote pacote = new Pacote();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    atendimento.idConsulta = Id;
                    atendimento.situacao = (String)(dr["SituacaoConsulta"]);
                    atendimento.dataHora = Convert.ToDateTime((dr["DiaHorario"]));
                    atendimento.dataCompra = Convert.ToDateTime((dr["DataCadastroConsulta"]));
                    atendimento.cliente.Id = Convert.ToInt32(dr["idCliente"]);

                    atendimento.cliente.CPF = dr["CPF_Cliente"] != DBNull.Value ? dr["CPF_Cliente"].ToString() : string.Empty;
                    atendimento.cliente.Nome = dr["NomeCliente"] != DBNull.Value ? dr["NomeCliente"].ToString() : string.Empty;
                    atendimento.cliente.Email = dr["EmailCliente"] != DBNull.Value ? dr["EmailCliente"].ToString() : string.Empty;
                    atendimento.cliente.Telefone = dr["TelefoneCliente"] != DBNull.Value ? dr["TelefoneCliente"].ToString() : string.Empty;
                    atendimento.cliente.Situacao = dr["SituacaoCliente"] != DBNull.Value ? dr["SituacaoCliente"].ToString() : string.Empty;
                    atendimento.cliente.dataCadastro = Convert.ToDateTime(dr["DataCadastroCliente"]);

                    if (dr["Matricula"] != DBNull.Value)
                    {
                        atendimento.funcionario.Matricula = Convert.ToInt32(dr["Matricula"]);
                        atendimento.funcionario.Cargo = dr["Funcao"] != DBNull.Value ? dr["Funcao"].ToString() : string.Empty;
                        atendimento.funcionario.CPF = dr["CPF_Funcionario"] != DBNull.Value ? dr["CPF_Funcionario"].ToString() : string.Empty;
                        atendimento.funcionario.Nome = dr["NomeFuncionario"] != DBNull.Value ? dr["NomeFuncionario"].ToString() : string.Empty;
                        atendimento.funcionario.Email = dr["EmailFuncionario"] != DBNull.Value ? dr["EmailFuncionario"].ToString() : string.Empty;
                        atendimento.funcionario.Telefone = dr["TelefoneFuncionario"] != DBNull.Value ? dr["TelefoneFuncionario"].ToString() : string.Empty;
                        atendimento.funcionario.Situacao = dr["SituacaoFuncionario"] != DBNull.Value ? dr["SituacaoFuncionario"].ToString() : string.Empty;

                    }

                    if (dr["idPagamento"] != DBNull.Value)
                    {
                        atendimento.pagamento.IdPagamento = Convert.ToInt32(dr["idPagamento"]);
                        atendimento.pagamento.Valor = dr["ValorPagamento"] != DBNull.Value ? Convert.ToDouble(dr["ValorPagamento"]) : 0.0;
                        atendimento.pagamento.Metodo = dr["MetodoPagamento"] != DBNull.Value ? dr["MetodoPagamento"].ToString() : string.Empty;
                        atendimento.pagamento.Status = dr["SituacaoPagamento"] != DBNull.Value ? dr["SituacaoPagamento"].ToString() : string.Empty;
                    }
                }
            }

            return atendimento;

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
