using PomoDeEris.Models;

namespace PomoDeEris.Areas.Colaborador.Models
{
    public class DashBoardViewModel
    {
        public int quantidadeConsultasHoje { get; set; }

        public double faturamentoDia { get; set; }
        public double faturamentoMes { get; set; }

        public double clientesCadastradosHoje { get; set; }

        public List<Atendimento> atendimentos { get; set; }

        public List<VendaPacote> vendasPacote { get; set; }

        public List<AtividadeRecente> atividadesRecentes { get; set; }

    }
}
