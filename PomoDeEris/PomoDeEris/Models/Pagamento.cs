namespace PomoDeEris.Models
{
    public class Pagamento
    {
        public int IdPagamento { get; set; }

        public string Metodo { get; set; }

        public double Valor { get; set; }

        public string Status { get; set; }
    }
}
