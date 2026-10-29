using PomoDeEris.Models.Constants;
using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class Funcionario : Pessoa
    {
        [Required]
        public FuncionarioCargoConstant cargo { get; set; }
    }
}
