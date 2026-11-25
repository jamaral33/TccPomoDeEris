using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Filtro;
using PomoDeEris.Models;
using PomoDeEris.Repository.Contract;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    [ColaboradorAutorizacaoAttribute]
    [Area("Colaborador")]
    public class ClienteController : Controller
    {
        private IClienteRepository _clienteRepository;
        private IAtendimentoRepository _atendimentoRepository;
        public ClienteController(IClienteRepository clienteRepository, IAtendimentoRepository atendimentoRepository)
        {
            _clienteRepository = clienteRepository;
            _atendimentoRepository = atendimentoRepository;
        }


        public IActionResult Index(string? clientePesquisa)
        {
            List<ClienteHistoricoViewModel> historico = new List<ClienteHistoricoViewModel>();
            var clientes = _clienteRepository.ObterTodosClientes();
            if (!string.IsNullOrWhiteSpace(clientePesquisa))
            {
                clientes = clientes.Where(c => c.Nome.Contains(clientePesquisa, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            foreach (var cliente in clientes)
            {
                List<Atendimento> clienteAtendimentos = new List<Atendimento>();
                clienteAtendimentos = _atendimentoRepository.ObterTodosAtendimentosCliente(cliente.Id);

                historico.Add(new ClienteHistoricoViewModel
                {
                    Cliente = cliente,
                    TotalGasto = clienteAtendimentos.Where(a => a.Situacao == "Concluido").Sum(a => a.Pagamento.Valor),
                    Sessoes = clienteAtendimentos.Count(),
                    UltimoAtendimento = clienteAtendimentos.Select(a => a.DataHora).OrderByDescending(a => a).FirstOrDefault()
                });
            }

            return View(historico);
        }

    }
}
