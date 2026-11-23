using PomoDeEris.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class Atendimento
    {
        [Required]
        public int IdConsulta { get; set; }

        [Required]
        public DateTime DataHora { get; set; }
        public DateTime DataCompra { get; set; }

        [Required]
        public Cliente Cliente { get; set; }

        public Funcionario Funcionario { get; set; }
        
        public string Situacao { get; set; }

        public List<Pacote> PacotesAtendimento { get; set; }

        public Pagamento Pagamento { get; set; }
    }
}
