using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class Pacote
    {
        public int IdPacote { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O nome do pacote deve ter no máximo 50 caracteres.")]
        public string Nome { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "A descrição do pacote deve ter no máximo 150 caracteres.")]
        public string Descricao { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public string Imagem { get; set; }
        

        public int Desconto { get; set; }

        public List<Servico> Servicos { get; set; }

        public int Quantidade { get; set; }
    }
}
