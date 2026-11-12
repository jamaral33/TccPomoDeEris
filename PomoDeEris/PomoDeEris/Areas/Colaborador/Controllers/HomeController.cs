using Microsoft.AspNetCore.Mvc;
using PomoDeEris.Libraries.Filtro;

namespace PomoDeEris.Areas.Colaborador.Controllers
{
    [Area("Colaborador")]
    public class HomeController : Controller
    {
        [ColaboradorAutorizacaoAttribute]
        public IActionResult Index()
        {
            return View();
        }
    }
}
