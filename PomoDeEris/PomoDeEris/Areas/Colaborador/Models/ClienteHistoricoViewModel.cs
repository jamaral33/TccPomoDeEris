using PomoDeEris.Models;

public class ClienteHistoricoViewModel
{
    public Cliente Cliente { get; set; }

    public DateTime? UltimoAtendimento { get; set; }

    public double TotalGasto { get; set; }

    public int Sessoes { get; set; }
}