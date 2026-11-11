using PomoDeEris.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class Funcionario : Pessoa
    {
        [Required]
        public string cargo { get; set; }
    }
}
