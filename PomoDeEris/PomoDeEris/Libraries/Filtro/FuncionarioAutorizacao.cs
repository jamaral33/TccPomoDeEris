using PomoDeEris.Libraries.Login;
using PomoDeEris.Models.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PomoDeEris.Libraries.Filtro
{
    public class ColaboradorAutorizacaoAttribute : Attribute, IAuthorizationFilter
    {
        private string _tipoColaboradorAutorizado;

        public ColaboradorAutorizacaoAttribute(string TipoColaboradorAutorizado = FuncionarioCargoConstant.Comum)
        {
            _tipoColaboradorAutorizado = TipoColaboradorAutorizado;
        }
        LoginFuncionario _loginFuncionario;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginFuncionario = (LoginFuncionario)context.HttpContext.RequestServices.GetService(typeof(LoginFuncionario));
            Models.Funcionario colaborador = _loginFuncionario.GetFuncionario();
            if (colaborador == null)
            {
                context.Result = new RedirectToActionResult("Login", "Colaborador", null);
            }
            else
            {
                if (colaborador.cargo == FuncionarioCargoConstant.Comum && _tipoColaboradorAutorizado == FuncionarioCargoConstant.Administrador)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}

