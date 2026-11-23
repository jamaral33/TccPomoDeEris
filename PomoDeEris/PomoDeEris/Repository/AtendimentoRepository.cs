using System.ComponentModel.DataAnnotations;
using System.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using PomoDeEris.Areas.Colaborador.Models;
using PomoDeEris.Libraries.Carrinho;
using PomoDeEris.Libraries.Criptografia;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

                cmd.Parameters.Add("@IdCliente", MySqlDbType.Int32).Value = atendimento.Cliente.Id;
                cmd.Parameters.Add("@Horario", MySqlDbType.DateTime).Value = atendimento.DataHora;
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

            atendimento.Cliente = new Cliente();
            atendimento.Funcionario = new Funcionario();
            atendimento.Pagamento = new Pagamento();
            atendimento.PacotesAtendimento = new List<Pacote>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(@"CALL sp_selecionar_informacoes_consulta(@idConsulta)", conexao);
                cmd.Parameters.Add("@idConsulta", MySqlDbType.Int32).Value = Id;

                MySqlDataReader dr;

                Pacote pacote = new Pacote();
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    atendimento.IdConsulta = Id;
                    atendimento.Situacao = (string)(dr["SituacaoConsulta"]);
                    atendimento.DataHora = Convert.ToDateTime((dr["DiaHorario"]));
                    atendimento.DataCompra = Convert.ToDateTime((dr["DataCadastroConsulta"]));
                    atendimento.Cliente.Id = Convert.ToInt32(dr["idCliente"]);

                    atendimento.Cliente.CPF = dr["CPF_Cliente"] != DBNull.Value ? dr["CPF_Cliente"].ToString() : string.Empty;
                    atendimento.Cliente.Nome = dr["NomeCliente"] != DBNull.Value ? dr["NomeCliente"].ToString() : string.Empty;
                    atendimento.Cliente.Email = dr["EmailCliente"] != DBNull.Value ? dr["EmailCliente"].ToString() : string.Empty;
                    atendimento.Cliente.Telefone = dr["TelefoneCliente"] != DBNull.Value ? dr["TelefoneCliente"].ToString() : string.Empty;
                    atendimento.Cliente.Situacao = dr["SituacaoCliente"] != DBNull.Value ? dr["SituacaoCliente"].ToString() : string.Empty;
                    atendimento.Cliente.dataCadastro = Convert.ToDateTime(dr["DataCadastroCliente"]);

                    if (dr["Matricula"] != DBNull.Value)
                    {
                        atendimento.Funcionario.Matricula = Convert.ToInt32(dr["Matricula"]);
                        atendimento.Funcionario.Cargo = dr["Funcao"] != DBNull.Value ? dr["Funcao"].ToString() : string.Empty;
                        atendimento.Funcionario.CPF = dr["CPF_Funcionario"] != DBNull.Value ? dr["CPF_Funcionario"].ToString() : string.Empty;
                        atendimento.Funcionario.Nome = dr["NomeFuncionario"] != DBNull.Value ? dr["NomeFuncionario"].ToString() : string.Empty;
                        atendimento.Funcionario.Email = dr["EmailFuncionario"] != DBNull.Value ? dr["EmailFuncionario"].ToString() : string.Empty;
                        atendimento.Funcionario.Telefone = dr["TelefoneFuncionario"] != DBNull.Value ? dr["TelefoneFuncionario"].ToString() : string.Empty;
                        atendimento.Funcionario.Situacao = dr["SituacaoFuncionario"] != DBNull.Value ? dr["SituacaoFuncionario"].ToString() : string.Empty;

                    }

                    if (dr["idPagamento"] != DBNull.Value)
                    {
                        atendimento.Pagamento.IdPagamento = Convert.ToInt32(dr["idPagamento"]);
                        atendimento.Pagamento.Valor = dr["ValorPagamento"] != DBNull.Value ? Convert.ToDouble(dr["ValorPagamento"]) : 0.0;
                        atendimento.Pagamento.Metodo = dr["MetodoPagamento"] != DBNull.Value ? dr["MetodoPagamento"].ToString() : string.Empty;
                        atendimento.Pagamento.Status = dr["SituacaoPagamento"] != DBNull.Value ? dr["SituacaoPagamento"].ToString() : string.Empty;
                    }


                }
                using (var conexao2 = new MySqlConnection(_conexaoMySQL))
                {
                    conexao2.Open();
                    MySqlCommand cmdPacotes = new MySqlCommand("call sp_selecionar_pacote_consulta(@idConsulta) ", conexao2);
                    cmdPacotes.Parameters.Add("@idConsulta", MySqlDbType.Int32).Value = Id;

                    MySqlDataAdapter da = new MySqlDataAdapter(cmdPacotes);
                    DataTable dt = new DataTable();

                    da.Fill(dt);
                    conexao.Close();
                    foreach (DataRow datarow in dt.Rows)
                    {
                        atendimento.PacotesAtendimento.Add(
                             new Pacote
                             {
                                 IdPacote = Convert.ToInt32(datarow["idPacote"]),
                                 Nome = (string)(datarow["Nome"]),
                                 Descricao = (string)(datarow["Descricao"]),
                                 Imagem = (string)(datarow["Imagem"]),
                                 Valor = Convert.ToDouble(datarow["Valor"]),
                                 Quantidade = Convert.ToInt32(datarow["Quantidade"])

                             });
                    }
                    conexao.Close();
                }
            }

            return atendimento;

        }
       

        public List<Atendimento> ObterTodosAtendimentos()
        {
            throw new NotImplementedException();
        }

        public int QuantidadeConsultaHoje()
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

        public double ReceitaDia()
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

        public double ReceitaMes()
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

        public int CadastrosHoje()
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

        public List<int> ObterIdsConsultasDia(DateTime data)
        {
            List<int> ids = new List<int>();
            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand(@"call sp_selecionar_consultas_dia(@Dia)", conexao);

                cmd.Parameters.Add("@Dia", MySqlDbType.DateTime).Value = data.Date;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        ids.Add(reader.GetInt32("IdConsulta"));
                }

            }

            return ids;
        }

        public List<Atendimento> ObterAtendimentosDia(DateTime data)
        {
            List<int> ids = ObterIdsConsultasDia(data);
            List<Atendimento> atendimentos = new List<Atendimento>();

            foreach (var id in ids) 
            {
                atendimentos.Add(ObterAtendimento(id));
            }

            return atendimentos;

        }

        public List<VendaPacote> ObterVendasPacotes()
        {
            List<VendaPacote> vendas = new List<VendaPacote>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                MySqlCommand cmd = new MySqlCommand("CALL sp_selecionar_vendas_pacotes()", conexao);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vendas.Add(new VendaPacote
                        {
                            IdPacote = Convert.ToInt32(reader["idPacote"]),
                            NomePacote = (string)reader["NomePacote"],
                            Vendas = Convert.ToInt32(reader["Vendas"]),
                            Receita = Convert.ToDouble(reader["Receita"])
                        });
                    }
                }
            }

            return vendas;
        }

        public List<AtividadeRecente> ObterAtividadeRecente()
        {
            List<AtividadeRecente> atividadeRecente = new List<AtividadeRecente>();

            using(var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                MySqlCommand cmd = new MySqlCommand("CALL sp_atividade_recente(5)", conexao);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AtividadeRecente atividade = new AtividadeRecente
                        {
                            Tipo = reader["Tipo"].ToString(),
                            NomeCliente = reader["NomeCliente"].ToString(),
                            DataEvento = Convert.ToDateTime(reader["DataEvento"])
                        };

                        if (reader["QuantidadePacotes"] != DBNull.Value)
                        {
                            atividade.QuantidadePacotes =
                                Convert.ToInt32(reader["QuantidadePacotes"]);
                        }

                        atividadeRecente.Add(atividade);
                    }
                }
                conexao.Close();
            }
            return atividadeRecente;

        }
    }
}
