namespace PomoDeEris.Models
{
    public class Cliente : Pessoa
    {
        public string? MetodoPagamento { get; set; }

        public DateTime dataCadastro { get; set; }
    }
}
    