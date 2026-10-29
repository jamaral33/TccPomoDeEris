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

        [Required]
        public Cliente cliente { get; set; }

        public Funcionario funcionario { get; set; }

        [Required]
        public AtendimentoSituacaoConstant situacao { get; set; }
    }
}
