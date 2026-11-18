using PomoDeEris.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class Atendimento
    {
        [Required]
        public int idConsulta { get; set; }

        [Required]
        public DateTime dataHora { get; set; }
        public DateTime dataCompra { get; set; }

        [Required]
        public Cliente cliente { get; set; }

        public Funcionario funcionario { get; set; }
        
        public string situacao { get; set; }

        public List<Pacote> pacotesAtendimento { get; set; }

        public Pagamento pagamento { get; set; }
    }
}
