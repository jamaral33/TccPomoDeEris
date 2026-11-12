using PomoDeEris.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class Funcionario : Pessoa
    {
        public int Matricula { get; set; }

        [Required]
        public string Cargo { get; set; }
    }
}
