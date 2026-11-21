using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class Servico
    {

        public int IdServico { get; set; }

        [MaxLength(50, ErrorMessage = "O nome do serviço não pode ultrapassar 50 caracteres.")]
        public string NomeServico { get; set; }


        [MaxLength(150, ErrorMessage = "A descrição do serviço não pode ultrapassar 150 caracteres.")]
        public string DescricaoServico { get; set; } 
    }
}
