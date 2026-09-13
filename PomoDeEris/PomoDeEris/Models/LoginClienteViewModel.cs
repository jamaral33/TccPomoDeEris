using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class LoginClienteViewModel
    {
        [Required(ErrorMessage ="O email não pode ser vazio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha não pode ser vazia")]
        public string Senha { get; set; }

    }
}
