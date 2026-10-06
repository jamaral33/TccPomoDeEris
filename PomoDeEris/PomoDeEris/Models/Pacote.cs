using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class Pacote
    {
        public int IdPacote { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Descricao { get; set; }

        [Required]
        public double Valor { get; set; }

        [Required]
        public string Imagem { get; set; }

    }
}
