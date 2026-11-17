namespace PomoDeEris.Models
{
    public class Pagamento
    {
        private int IdPagamento { get; set; }

        private string Metodo { get; set; }

        private double Valor { get; set; }

        private string Status { get; set; }
    }
}
