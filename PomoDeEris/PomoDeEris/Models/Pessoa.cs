using System.ComponentModel.DataAnnotations;

namespace PomoDeEris.Models
{
    public class Pessoa
    {
        public int Id { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "O CPF é obrigatório")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 caracteres")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números e ter  11 dígitos")]
        public string? CPF { get; set; }

        [Display(Name = "Nome completo", Description = "Nome e Sobrenome.")]
        [Required(ErrorMessage = "O nome completo é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter até 100 caracteres")]

        public string? Nome { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = " O Email não é válido")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "O email deve possuir um domínio válido")]
        public string? Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "O senha é obrigatório")]
        public string? Senha { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "O Telefone é obrigatório")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O telefone deve conter apenas números e ter  11 dígitos")]
        public string? Telefone { get; set; }




    }
}
